using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitrator : MonoBehaviour
{
    [SerializeField] private Board _board;
    [SerializeField] private MyGridVisual _visual;
    [SerializeField] private List<LevelSO> _levelData;

    private void Start()
    {
        Board board = Instantiate(_board);
        MyGridVisual visual = Instantiate(_visual);

        board.BoardSetUp(_levelData[0], _visual);
    }
}
