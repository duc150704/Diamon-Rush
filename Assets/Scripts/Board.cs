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

    private MyGrid _Grid;
    private HashSet<GridCell> _matchedCells  = new();

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
                
            _cellSelected = _Grid.TryGetCell(InputManager.Instance.GetMousePostion(), out _selected);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && _selected != null && _selected.HasDiamon())
        {
            _dragDir = InputManager.Instance.GetMousePostion() - _Grid.GridToWorld(_selected.GridPos);
            if (_dragDir.magnitude > _cellSize * 0.5f + 0.1f)
            {
                Swap(_selected.Diamond, NormalizeDragDir());
            }
                
        }
    }

    private Direction NormalizeDragDir()
    {
        _dragDir.Normalize();
        float angle = Mathf.Atan2(_dragDir.y, _dragDir.x) * Mathf.Rad2Deg;
        Direction dir = Direction.None;
        
        if(-30 <= angle && angle <= 30)
        {
            dir = Direction.Right;
        } 
        else if (60 <= angle && angle <= 120)
        {
            dir = Direction.Up;
        }
        else if (-120 <= angle && angle <= -60)
        {
            dir = Direction.Down;
        }
        else if (angle >= 150 || angle <= -150)
        {
            dir = Direction.Left;
        }

        return dir;
    }

    private async void Swap(Diamon diamon, Direction direction)
    {
        if (_isSwapping)
            return;

        if (!_Grid.TryGetNeighbor(diamon.GridPos, direction, out Diamon neighbor))
            return;

        _isSwapping = true;

        _Grid.Swap(diamon.GridPos, neighbor.GridPos);
        await _gridVisual.SwapAnimation(neighbor, diamon);
        bool c = Check();
        if (c == false)
        {
            _Grid.Swap(diamon.GridPos, neighbor.GridPos);
            await _gridVisual.SwapAnimation(neighbor, diamon);
            _isSwapping = false;
            return;
        }

        while(Check())
        {
            await _gridVisual.DisapearAnimate(_matchedCells);
            ClearMatched(_matchedCells);
            FallDown();
            await _gridVisual.FallDown();
            
            HashSet<RefillData> a = Refill();
            await _gridVisual.RefillAnimation(a);

            _matchedCells.Clear();
        }
        //VerticalCheck();
        //HorizontalCheck();
        

        _isSwapping = false;
    }

    public bool Check()
    {
        bool a = VerticalCheck();
        bool b = HorizontalCheck();
        return a || b;
    }

    public void ClearMatched(HashSet<GridCell> cells)
    {
        foreach (var item in cells)
        {
            _Grid.SetDiamond(item.GridPos, null);
        }
    }

    private void InitBoard()
    {
        _Grid = new MyGrid(_width, _height, _cellSize, _centerPos, _offset);
        InitGrid();
        _gridVisual.Init(_Grid);
    }

    private void InitGrid()
    {
        Vector2Int gridPos;
        for (int i = 0; i < _Grid.Width; i++)
        {
            for (int j = 0; j < _Grid.Height; j++)
            {
                int index = UnityEngine.Random.Range(0, _diamondData.Length );
                gridPos = new Vector2Int(i, j);
                _Grid.SetDiamond(gridPos, new Diamon(gridPos, _diamondData[index]));
            }
        }
    }

    private bool VerticalCheck()
    {
        int z = 0;
        for(int i = 0; i < _Grid.Width; i++)
        {
            int idx = 1;
            for(int j = 1; j < _Grid.Height; j++)
            {
                GridCell cur = _Grid.GetCell(new Vector2Int(i, j));
                GridCell pre = _Grid.GetCell(new Vector2Int(i, j - 1));

                if (cur.Diamond == null || pre.Diamond == null)
                    continue;

                if(cur.Diamond.Type == pre.Diamond.Type)
                {
                    idx++;
                } else
                {
                    if(idx >= 3)
                    {
                        for(int k = 1; k <= idx; k++)
                        {
                            _matchedCells.Add(_Grid.GetCell(new Vector2Int(i, j - k)));
                        }
                        z++;
                    }
                    idx = 1;
                }
            }
            if (idx >= 3)
            {
                for (int k = 1; k <= idx; k++)
                {
                    _matchedCells.Add(_Grid.GetCell(new Vector2Int(i, _Grid.Height - k)));
                    z++;
                }
            }
        }
        return z > 0;
    }

    private bool HorizontalCheck()
    {
        int z = 0;
        for (int i = 0; i < _Grid.Height; i++)
        {
            int idx = 1;
            for (int j = 1; j < _Grid.Width; j++)
            {
                GridCell cur = _Grid.GetCell(new Vector2Int(j, i));
                GridCell pre = _Grid.GetCell(new Vector2Int(j - 1, i));

                if (cur.Diamond == null || pre.Diamond == null)
                    continue;

                if (cur.Diamond.Type == pre.Diamond.Type)
                {
                    idx++;
                }
                else
                {
                    if (idx >= 3)
                    {
                        for (int k = 1; k <= idx; k++)
                        {
                            _matchedCells.Add(_Grid.GetCell(new Vector2Int(j - k, i)));
                        }
                        z++;
                    }
                    idx = 1;
                }
            }

            if (idx >= 3)
            {
                for (int k = 1; k <= idx; k++)
                {
                    _matchedCells.Add(_Grid.GetCell(new Vector2Int(_Grid.Width - k, i)));
                }
                z++;
            }
        }
        return z > 0;
    }

    private void FallDown()
    {
        for (int i = 0; i < _Grid.Width; i++) 
        {
            int down = 0;
            int up = 0;
            while (up < _Grid.Height)
            {
                GridCell cur = _Grid.GetCell(new Vector2Int(i, up));
                GridCell pre = _Grid.GetCell(new Vector2Int(i, down));

                if(cur.Diamond == null)
                {
                    up++;
                    continue;
                }
                if(up != down)
                {
                    _Grid.SetDiamond(new Vector2Int(i, down), cur.Diamond);
                    _Grid.SetDiamond(new Vector2Int(i, up), null);
                }

                down++;
                up++;
            }
        }
    }

    private HashSet<RefillData> Refill()
    {
        HashSet<RefillData> data = new();
        for(int i = 0; i < _Grid.Width; i++)
        {
            for(int j = _Grid.Height - 1; j >= 0; j--)
            {
                GridCell cell = _Grid.GetCell(new Vector2Int(i, j));

                if (cell.HasDiamon())
                    break;

                int index = UnityEngine.Random.Range(0, _diamondData.Length);
                RefillData rd = new RefillData() 
                {
                    Diamon = new Diamon(new Vector2Int(i, j), _diamondData[index]),
                    RespawnPosition = _Grid.GridToWorld(new Vector2Int(i, j + _Grid.Height))
                };

                _Grid.SetDiamond(new Vector2Int(i, j), rd.Diamon);

                data.Add(rd);
                //;
                //_Grid.SetDiamond(new Vector2Int(i, j), new Diamon();
                //await _gridVisual.Visualize(_Grid.GetCell(new Vector2Int(i, j)).Diamond);
            }
        }
        return data;
        
    }
}
