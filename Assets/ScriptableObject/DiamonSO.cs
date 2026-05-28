using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiamondTypeExtension
{
    private static System.Random random = new System.Random();
    public static EDiamonType Next(int next)
    { 
        Array types = Enum.GetValues(typeof(EDiamonType));

        if(next >= types.Length)
            next = types.Length;

        return (EDiamonType)types.GetValue(random.Next(next));
    }
}

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
