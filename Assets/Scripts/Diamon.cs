using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamon : IPoolable
{
    private Vector2Int _gridPos;
    private System.Random random = new System.Random();

    public int X { get => _gridPos.x; set => _gridPos.x = value; }
    public int Y { get => _gridPos.y; set => _gridPos.y = value; }
    public bool IsMatch {  get; private set; }
    public EDiamonType Type { get; set; }
    public Vector2Int GridPos
    {
        get => _gridPos;
        set {
            PreviousGridPos = _gridPos;
            _gridPos = value;
        }
    }
    public Vector2Int PreviousGridPos {  get; set; }
    public bool IsActive { get; private set; }

    public Diamon(Vector2Int gridPos , DiamonSO data)
    {
        IsActive = true;
        _gridPos = gridPos; 
        Type = data.Type;
    }

    public Diamon(int x, int y, EDiamonType type)
    {
        X = x;
        Y = y;
        Type = type;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    //public void OnActivate()
    //{
    //    Array types = Enum.GetValues(typeof(EDiamonType));
    //    Type = (EDiamonType)types.GetValue(random.Next(types.Length));
    //}
}
