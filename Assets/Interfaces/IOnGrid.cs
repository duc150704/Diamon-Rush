using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnGrid
{
    public Vector2Int GridPos { get; }
    public int X { get; set; }
    public int Y { get; set; }
}

