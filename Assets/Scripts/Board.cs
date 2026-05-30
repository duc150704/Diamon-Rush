using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private MyGridVisual _gridVisual;
    private MyGrid _grid;
    private LevelSO _levelSO;

    private bool _isSwapping = false;
    private GridCell _selected;
    private Vector3 _dragDir;

    private int _score = 0;
    private List<EDiamonType> _currentDiamondTypes = new();
 
    public static event Action<int> OnScoreChanged;

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
            _grid.TryGetCell(InputManager.Instance.GetMousePostion(), out _selected);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && _selected != null && _selected.HasActivateDiamon())
        {
            _dragDir = InputManager.Instance.GetMousePostion() - _grid.GridToWorld(_selected.GridPos);
            if (_dragDir.magnitude > _levelSO.CellSize * 0.5f + 0.1f)
            {
                Swap(_selected.Diamond, DirectionCalculator.Normolize(_dragDir));
            }
                
        }
    }

    public void BoardSetUp(LevelSO levelSO, MyGridVisual gridVisual)
    {
        _levelSO = levelSO;
        this._gridVisual = gridVisual;

        _grid = new MyGrid(levelSO.Width, levelSO.Height, levelSO.CellSize, levelSO.CenterPosition, levelSO.Offset);
        InitGrid();
        gridVisual.Init(_grid, levelSO.Diamonds.ToArray());
        GetTypes();
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

        CheckedResult matched = BoardLogic.FindMatches(_grid);

        if (!matched.HasAnyMatched)
        {
            _grid.SwapDiamon(diamon.GridPos, neighbor.GridPos);
            await _gridVisual.SwapAnimation(neighbor, diamon);
            _isSwapping = false;
            return;
        }

        int comboChain = 1;
        while(matched.HasAnyMatched)
        {
            await _gridVisual.DisapearAnimate(matched.Diamons);

            _score += BoardLogic.GetScore(matched, comboChain);
            OnScoreChanged?.Invoke(_score);

            BoardLogic.ClearMatched(matched.Diamons);

            BoardLogic.ApplyGravity(_grid);
            await _gridVisual.FallDown();

            HashSet<RefillData> a = BoardLogic.Refill(_grid, _currentDiamondTypes);
            await _gridVisual.RefillAnimation(a);

            comboChain++;
            matched = BoardLogic.FindMatches(_grid);
        }
        WinChecker.Check(new WinCheckData() { Score = _score });
        _isSwapping = false;
    }

    private void InitGrid()
    {
        Vector2Int gridPos;
        for (int i = 0; i < _grid.Width; i++)
        {
            for (int j = 0; j < _grid.Height; j++)
            {
                int index = UnityEngine.Random.Range(0, _levelSO.Diamonds.Count );
                gridPos = new Vector2Int(i, j);
                _grid.SetDiamond(gridPos, new Diamon(gridPos, _levelSO.Diamonds[index]));
            }
        }
    }

    private void GetTypes()
    {
        foreach (var item in _levelSO.Diamonds)
        {
            _currentDiamondTypes.Add(item.Type);
        }
    }
}
