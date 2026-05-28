using UnityEngine;

public class MyGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }

    private GridCell[,] _cells;
    private Vector3 _center;

    public MyGrid(int width, int height, float cellSize, Vector3 centerPos = default(Vector3), Vector3 offset = default)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        _cells = new GridCell[Width, Height];
        InitGridCell();
        _center = centerPos + offset;
    }

    private void InitGridCell()
    {
        for(int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                _cells[i, j] = new GridCell(new Vector2Int(i,j));
            }
        }
    }

    public bool SwapDiamon(Vector2Int from, Vector2Int to)
    {
        if (!IsValid(from) || !IsValid(to))
            return false;

        if (_cells[from.x, from.y].IsBlocked || _cells[to.x, to.y].IsBlocked) 
            return false;

        if (!_cells[from.x, from.y].HasActiveDiamon() || !_cells[to.x, to.y].HasActiveDiamon()) // iugguhohfufutf
            return false;

        Diamon tmp = _cells[from.x, from.y].Diamond;

        SetDiamond(from, GetCell(to).Diamond);
        SetDiamond(to, tmp);
        return true;
    }

    public bool SwapDiamon(int x1, int y1, int x2, int y2)
    {
        if (!IsValid(x1, y1) || !IsValid(x2, y2))
            return false;

        if (_cells[x1, y1].IsBlocked || _cells[x2, y2].IsBlocked)
            return false;

        if (_cells[x1, y1].Diamond == null || _cells[x2, y2].Diamond == null) // diamon khong duocj null
            return false;

        Diamon tmp = _cells[x1, y1].Diamond;

        SetDiamond(x1, y1, GetCell(x2, y2).Diamond);
        SetDiamond(x2, y2, tmp);
        return true;
    }

    public bool TryGetNeighbor(Vector2Int pos, Direction direction, out Diamon neighbor)
    {
        Vector2Int neiPos = Vector2Int.zero;
        neighbor = null;

        switch (direction)
        {
            case Direction.Left:
                neiPos = Vector2Int.left;
                break;
            case Direction.Right:
                neiPos = Vector2Int.right;
                break;
            case Direction.Up:
                neiPos = Vector2Int.up;
                break;
            case Direction.Down:
                neiPos = Vector2Int.down;
                break;
        }

        GridCell cell = GetCell(pos + neiPos);
        if (cell == null || !cell.HasActiveDiamon())
            return false;

        neighbor = cell.Diamond;
        return neighbor != null && neiPos != Vector2Int.zero;
    }

    private Vector3 CaculateCenter()
    {
        return new Vector3((Width * CellSize) / 2f, (Height * CellSize) / 2f); 
    }

    public bool TryGetCell(Vector3 worldPos, out GridCell cell)
    {
        cell = GetCell(WorldToGrid(worldPos));
        return cell != null;
    }

    public bool SetDiamond(Vector2Int gridPos, Diamon value)
        => SetDiamond(gridPos.x, gridPos.y, value);


    public bool SetDiamond(int x, int y, Diamon value)
    {
        if(!IsValid(x, y))
            return false;

        _cells[x, y].Diamond = value;

        if(value != null) // cell chap nhan null diamon
        {
            _cells[x, y].Diamond.X = x;
            _cells[x, y].Diamond.Y = y;
        }

        return true;
            
    }

    public bool SetDiamond(Vector3 worldPos, Diamon value)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);

        return SetDiamond(gridPos, value);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        Vector3 pos = new Vector3();
        pos.x = gridPos.x * CellSize + CellSize * 0.5f;
        pos.y = gridPos.y * CellSize + CellSize * 0.5f;

        return pos + (_center - CaculateCenter());
    }

    public bool TryWorldToGrid(Vector3 worldPos, out Vector2Int gridPos)
    {
        gridPos = WorldToGrid(worldPos);
        return IsValid(gridPos);
    }

    public GridCell GetCell(Vector2Int gridPos)
    {
        if (!IsValid(gridPos))
            return null;
        return _cells[gridPos.x, gridPos.y];
    }

    public GridCell GetCell(int x, int y)
    {
        if(!IsValid(x, y))
            return null;
        return _cells[x, y];
    }

    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector2Int gridPos = new Vector2Int();
        gridPos.x = Mathf.FloorToInt((worldPos.x - _center.x + CaculateCenter().x ) / CellSize);
        gridPos.y = Mathf.FloorToInt((worldPos.y - _center.y + CaculateCenter().y) / CellSize);
        return gridPos;
    }

    private bool IsValid(Vector2Int gridPos)
        => IsValid(gridPos.x, gridPos.y);

    private bool IsValid(int x, int y)
        => x >= 0 && y >= 0 && x < Width && y < Height;
}
