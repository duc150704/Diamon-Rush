using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellSize;

    [SerializeField] private DiamonSO[] _diamondData;
    [SerializeField] private MyGridVisual _gridVisual;

    private MyGrid<Diamon> _diamonGrid;


    private void Start()
    {
        _diamonGrid = new MyGrid<Diamon>(_width, _height, _cellSize);
        InitBoard();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (_diamonGrid.TryGetValue(Camera.main.ScreenToWorldPoint(Input.mousePosition), out Diamon diamon)){
                Debug.Log($"{diamon.Type}");
            }
        }
    }

    private void InitBoard()
    {
        InitGrid();
        _gridVisual.Init(_diamonGrid);
    }

    private void InitGrid()
    {
        Vector2Int gridPos;
        for (int i = 0; i < _diamonGrid.Width; i++)
        {
            for (int j = 0; j < _diamonGrid.Height; j++)
            {
                int index = UnityEngine.Random.Range(0, _diamondData.Length - 1);
                gridPos = new Vector2Int(i, j);
                _diamonGrid.SetValue(gridPos, new Diamon(gridPos, _diamondData[index]));
            }
        }
    }
}
