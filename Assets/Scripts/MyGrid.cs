using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid<T> where T : class, IOnGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }

    private T[,] _value;
    private Vector3 _center;
    private Vector3 _offset;

    public MyGrid(int width, int height, float cellSize, Vector3 centerPos = default(Vector3), Vector3 offset = default(Vector3))
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        _value = new T[Width, Height];
        _center = centerPos;
        _offset = offset;
    }

    private Vector3 CaculateCenter()
    {
        Vector3 tmp = new Vector3();
        tmp.x = (Width % 2 == 0) ? (Width / 2)  : (Width / 2) + CellSize * 0.5f; 
        tmp.y = (Height % 2 == 0) ? (Height/ 2)  : (Height/ 2) + CellSize * 0.5f; 

        return tmp;
    }

    public bool TryGetValue(Vector3 worldPos, out T value)
    {
        value = GetValue(WorldToGrid(worldPos));
        return value != null;
    }

    public bool SetValue(Vector2Int gridPos, T value)
    {
        if (!IsValid(gridPos))
            return false;

        _value[gridPos.x, gridPos.y] = value;

        if (value != null)
            _value[gridPos.x, gridPos.y].GridPos = gridPos;

        return true;
    }

    public bool SetValue(Vector3 worldPos, T value)
    {
        Vector2Int gridPos = WorldToGrid(worldPos);

        return SetValue(gridPos, value);
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        Vector3 pos = new Vector3();
        pos.x = gridPos.x * CellSize + CellSize * 0.5f;
        pos.y = gridPos.y * CellSize + CellSize * 0.5f;

        return pos + (_center - CaculateCenter());
    }

    public bool TryGetGridPos(Vector3 worldPos, out Vector2Int gridPos)
    {
        gridPos = WorldToGrid(worldPos);
        return IsValid(gridPos);
    }

    public T GetValue(Vector2Int gridPos)
    {
        if (!IsValid(gridPos))
            return null;
        return _value[gridPos.x, gridPos.y];
    }

    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector2Int gridPos = new Vector2Int();
        gridPos.x = Mathf.FloorToInt((worldPos.x / CellSize) + CaculateCenter().x);
        gridPos.y = Mathf.FloorToInt(worldPos.y / CellSize + CaculateCenter().y);
        return gridPos;
    }

    private bool IsValid(Vector2Int gridPos) 
        => gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x < Width && gridPos.y < Height;

}
