using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDiamonType
{
    D1, D2, D3, D4, D5, D6, D7, D8, D9
}

[CreateAssetMenu(fileName = "DiamonSO", menuName = "Diamon Data")]
public class DiamonSO : ScriptableObject
{
    public Sprite Sprite;
    public EDiamonType Type;
}
