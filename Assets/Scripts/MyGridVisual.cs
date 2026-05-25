using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MyGridVisual : MonoBehaviour
{
    [SerializeField] private GameObject _pref;
    private MyGrid<Diamon> _diamondGrid;

    public void Init(MyGrid<Diamon> myGrid)
    {
        _diamondGrid = myGrid;
        for(int i = 0; i < _diamondGrid.Width; i++)
        {
            for( int j = 0;j < _diamondGrid.Height; j++)
            {
                GameObject obj = Instantiate(_pref);
                DiamondVisual visual = obj.GetComponent<DiamondVisual>();
                visual.Visualize(_diamondGrid.GetValue(new Vector2Int(i, j)).Data, _diamondGrid.GridToWorld(new Vector2Int(i, j)));
            }
        }
    }
}
