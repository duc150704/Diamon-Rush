using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : IOnGrid
{
    public Vector2Int GridPos { get; private set;}
    public Diamon Diamond;
    public bool IsBlocked = false;

    public GridCell(Vector2Int gridPos)
    {
        GridPos = gridPos;
    }

    public bool HasDiamon()
        => this.Diamond != null;
}
