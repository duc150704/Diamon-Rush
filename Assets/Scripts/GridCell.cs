using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : IOnGrid
{
    private Vector2Int _gridPos;
    public Vector2Int GridPos
    {
        get => _gridPos;
        private set => _gridPos = value;
    }
    public int X { get => _gridPos.x; set => _gridPos.x = value; }
    public int Y { get => _gridPos.y; set => _gridPos.y = value; }

    public Diamon Diamond;
    public bool IsBlocked = false;

    public GridCell(Vector2Int gridPos)
    {
        _gridPos = gridPos;
    }

    public bool HasActiveDiamon()
        => this.Diamond.IsActive;
}
