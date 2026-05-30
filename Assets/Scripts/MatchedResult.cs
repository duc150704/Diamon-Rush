using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchedResult
{
//    public HashSet<GridCell> Cells = new();
//    public int GetQuantity()
//        => Cells.Count;  

//    public int GetScore()
//    {
//        return Score.Calculate(Cells.Count);
//    }
}

public class CheckedResult : IPoolable
{
    public List<MatchedData> MatchedDatas { get; } = new List<MatchedData>(5);
    public HashSet<Diamon> Diamons { get; } = new HashSet<Diamon>();
    public bool HasAnyMatched => MatchedDatas.Count > 0;

    public bool IsActive { get; private set; } = false;

    public void AddData(MatchedData data)
    {
        MatchedDatas.Add(data);
    }

    public void AddDiamond(Diamon diamond)
    {
        Diamons.Add(diamond);
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        MatchedDatas.Clear();
        Diamons.Clear();
        IsActive = false;
    }

    public int GetAllScore()
    {
        int score = 0;
        foreach (var item in MatchedDatas)
        {
            score += item.GetScore();
        }
        return score;
    }
}

public class MatchedData : IPoolable
{
    public List<Diamon> Diamonds { get; } = new(10);
    public bool IsActive { get; private set; }

    public int XRoot = 0;
    public int YRoot = 0;
    public MatchedShape MatchedShape = MatchedShape.None;
    
    public int GetScore()
    {
        return Score.Calculate(Diamonds.Count);
    }

    public void AddDiamond(Diamon diamon)
    {
        Diamonds.Add(diamon);
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
        Reset();
    }

    public void Reset()
    {
        Diamonds.Clear();
        XRoot = 0;
        YRoot = 0;
        MatchedShape = MatchedShape.None;
    }
}

public enum MatchedShape
{
    None,
}
