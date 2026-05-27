using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamon : IPoolable
{
    private Vector2Int _gridPos;

    public DiamonSO Data { get; private set; }
    public bool IsMatch {  get; private set; }
    public EDiamonType Type { get; private set; }
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
        Data = data;
        Type = data.Type;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
