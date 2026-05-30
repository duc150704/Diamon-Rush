using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private void Start()
    {
        Board.OnScoreChanged += HandleScoreChanged;
    }
    public void HandleScoreChanged(int Score)
    {
        _text.text = Score.ToString();
    }
}