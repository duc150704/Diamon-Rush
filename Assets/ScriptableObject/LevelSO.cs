using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level")]
public class LevelSO : ScriptableObject
{
    [Header("Level ID")]
    public int LevelID;

    [Header("Board Information")]
    public int Width;
    public int Height;
    public float CellSize;
    public Vector3 CenterPosition;
    public Vector3 Offset;

    [Header("Win Condition")]
    public int TargetScore;
    public int TimeLimit;

    [Header("Diamond Type")]
    public List<DiamonSO> Diamonds;
}
