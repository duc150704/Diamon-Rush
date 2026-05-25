using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamon : IPoolable, IOnGrid
{
    public DiamonSO Data { get; private set; }
    public bool IsMatch {  get; private set; }
    public EDiamonType Type { get; private set; }
    public Vector2Int GridPos { get; set; }
    public bool IsActive { get; private set; }

    public Diamon(Vector2Int gridPos , DiamonSO data)
    {
        GridPos = gridPos; 
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
