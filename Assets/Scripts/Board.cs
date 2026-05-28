using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField] private float _cellSize;

    [SerializeField] private Vector3 _centerPos;
    [SerializeField] private Vector3 _offset;

    [SerializeField] private DiamonSO[] _diamondData;
    [SerializeField] private MyGridVisual _gridVisual;

    private MyGrid _grid;

    private bool _isSwapping = false;

    private GridCell _selected;
    private bool _cellSelected;
    private Vector3 _dragDir;


    private void Start()
    {
        InitBoard();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_isSwapping)
            {
                _selected = null;
                _dragDir = Vector3.zero;
                return;
            }
                
            _cellSelected = _grid.TryGetCell(InputManager.Instance.GetMousePostion(), out _selected);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && _selected != null && _selected.HasActiveDiamon())
        {
            _dragDir = InputManager.Instance.GetMousePostion() - _grid.GridToWorld(_selected.GridPos);
            if (_dragDir.magnitude > _cellSize * 0.5f + 0.1f)
            {
                Swap(_selected.Diamond, DirectionCalculator.Normolize(_dragDir));
            }
                
        }
    }

    private async void Swap(Diamon diamon, Direction direction)
    {
        if (_isSwapping)
            return;

        if (!_grid.TryGetNeighbor(diamon.GridPos, direction, out Diamon neighbor))
            return;

        _isSwapping = true;

        _grid.SwapDiamon(diamon.GridPos, neighbor.GridPos);
        await _gridVisual.SwapAnimation(neighbor, diamon);
        HashSet<GridCell> matched = BoardLogic.FindMatches(_grid);
        if (matched.Count == 0)
        {
            _grid.SwapDiamon(diamon.GridPos, neighbor.GridPos);
            await _gridVisual.SwapAnimation(neighbor, diamon);
            _isSwapping = false;
            return;
        }

        while(matched.Count > 0)
        {
            await _gridVisual.DisapearAnimate(matched);
            ClearMatched(matched);
            BoardLogic.ApplyGravity(_grid);
            await _gridVisual.FallDown();
            
            HashSet<RefillData> a = BoardLogic.Refill(_grid);
            await _gridVisual.RefillAnimation(a);
            matched = BoardLogic.FindMatches(_grid);
        }

        _isSwapping = false;
    }

    public void ClearMatched(HashSet<GridCell> cells)
    {
        foreach (var item in cells)
        {
            //_grid.SetDiamond(item.GridPos, null);
            _grid.GetCell(item.GridPos).Diamond.Deactivate();
        }
    }

    private void InitBoard()
    {
        _grid = new MyGrid(_width, _height, _cellSize, _centerPos, _offset);
        InitGrid();
        _gridVisual.Init(_grid, _diamondData);
    }

    private void InitGrid()
    {
        Vector2Int gridPos;
        for (int i = 0; i < _grid.Width; i++)
        {
            for (int j = 0; j < _grid.Height; j++)
            {
                int index = UnityEngine.Random.Range(0, _diamondData.Length );
                gridPos = new Vector2Int(i, j);
                _grid.SetDiamond(gridPos, new Diamon(gridPos, _diamondData[index]));
            }
        }
    }
}
