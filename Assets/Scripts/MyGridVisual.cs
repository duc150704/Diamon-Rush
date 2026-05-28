using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MyGridVisual : MonoBehaviour
{
    [SerializeField] private GameObject _pref;
    [SerializeField] private float _swapTime = 0.5f;

    private MyGrid _diamondGrid;
    private Dictionary<Diamon, DiamondVisual> _visualMap = new();
    private Dictionary<EDiamonType, DiamonSO> _dataMap = new();

    private DiamonSO[] diamonSOs;
    public void Init(MyGrid myGrid, DiamonSO[] diamonSOss)
    {
        _diamondGrid = myGrid;
        diamonSOs = diamonSOss;
        InitData();

        for (int i = 0; i < _diamondGrid.Width; i++)
        {
            for( int j = 0;j < _diamondGrid.Height; j++)
            {
                GameObject obj = Instantiate(_pref);
                DiamondVisual visual = obj.GetComponent<DiamondVisual>();
                _visualMap[_diamondGrid.GetCell(new Vector2Int(i, j)).Diamond] = visual;
                EDiamonType tmp = _diamondGrid.GetCell(i, j).Diamond.Type;
                visual.Visualize(_dataMap[tmp], _diamondGrid.GridToWorld(new Vector2Int(i, j)));
            }
        }
    }

    private void InitData()
    {
        for(int i = 0;i < diamonSOs.Length; i++)
        {
            _dataMap[diamonSOs[i].Type] = diamonSOs[i];
        }
    }

    public async UniTask RefillAnimation(HashSet<RefillData> data)
    {
        List<UniTask> tasks = new List<UniTask>();
        foreach (var item in data)
        {
            GameObject obj = Instantiate(_pref);
            DiamondVisual visual = obj.GetComponent<DiamondVisual>();
            _visualMap[item.Cell.Diamond] = visual;
            visual.Visualize(_dataMap[item.Cell.Diamond.Type], item.RespawnPosition);
            tasks.Add(visual.Move(_diamondGrid.GridToWorld(item.Cell.Diamond.GridPos), 0.25f));
        }
        
        await UniTask.WhenAll(tasks);
    }

    public async UniTask SwapAnimation(Diamon objA, Diamon objB)
    {
        Vector3 aPos = _visualMap[objA].transform.position;
        Vector3 bPos = _visualMap[objB].transform.position;

        await UniTask.WhenAll(
            _visualMap[objA].Move(bPos, _swapTime),
            _visualMap[objB].Move(aPos, _swapTime)
            );
    }

    public async UniTask DisapearAnimate(HashSet<GridCell> _cell)
    {
        List<UniTask> uniTask = new();
        foreach (var item in _cell)
        {
            uniTask.Add(_visualMap[item.Diamond].PlayDisapearAnimation());
            
        }
        await UniTask.WhenAll(uniTask);
        foreach (var item in _cell)
        {
            _visualMap[item.Diamond].Deactivate();
        }

        await UniTask.Yield();
    }

    public async UniTask FallDown()
    {
        List<UniTask> uniTask = new ();
        for (int i = 0; i < _diamondGrid.Width; i++)
        {
            for (int j = 0; j < _diamondGrid.Height; j++)
            {
                Diamon a = _diamondGrid.GetCell(new Vector2Int(i, j)).Diamond;
                if (!a.IsActive)
                    continue;
                uniTask.Add(_visualMap[a].Move(_diamondGrid.GridToWorld(a.GridPos), _swapTime));
            }
        }

        await UniTask.WhenAll(uniTask);
    }
}
