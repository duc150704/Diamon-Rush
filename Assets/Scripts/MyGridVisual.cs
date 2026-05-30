using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class MyGridVisual : MonoBehaviour
{
    [SerializeField] private GameObject _pref;
    [SerializeField] private float _swapTime = 0.5f;

    private MyGrid _diamondGrid;
    private Dictionary<Diamon, DiamondVisual> _visualMap = new();
    private Dictionary<EDiamonType, DiamonSO> _dataMap = new();

    private DiamonSO[] _diamonSOs;
    public void Init(MyGrid myGrid, DiamonSO[] diamonSO)
    {
        _diamondGrid = myGrid;
        _diamonSOs = diamonSO;
        InitData();

        for (int i = 0; i < _diamondGrid.Width; i++)
        {
            for( int j = 0;j < _diamondGrid.Height; j++)
            {
                GameObject obj = Instantiate(_pref);
                DiamondVisual visual = obj.GetComponent<DiamondVisual>();
                _visualMap[_diamondGrid.GetCell(new Vector2Int(i, j)).Diamond] = visual;
                EDiamonType tmp = _diamondGrid.GetCell(i, j).Diamond.Type;
                visual.Visualize(_dataMap[tmp], _diamondGrid.GridToWorld(new Vector2Int(i, j)) + new Vector3(5, 0,0));
            }
        }

        UniTask.Delay(2000);
        BackToRightPostion().Forget();
    }

    public async UniTask BackToRightPostion()
    {
        List<UniTask> task = new();
        foreach (var item in _visualMap)
        {
            task.Add(item.Value.Move(_diamondGrid.GridToWorld(item.Key.GridPos), 0.5f));
        }

        await UniTask.WhenAll(task);
    }

    private void InitData()
    {
        for (int i = 0; i < _diamonSOs.Length; i++)
        {
            _dataMap[_diamonSOs[i].Type] = _diamonSOs[i];
        }
    }

    public async UniTask RefillAnimation(HashSet<RefillData> data)
    {
        List<UniTask> tasks = new List<UniTask>();
        foreach (var item in data)
        {
            DiamondVisual visual = _visualMap[item.Cell.Diamond];
            visual.Visualize(_dataMap[item.Cell.Diamond.Type], item.RespawnPosition);
            visual.Activate();
            tasks.Add(visual.Move(_diamondGrid.GridToWorld(item.Cell.GridPos), 0.25f));
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

    public async UniTask DisapearAnimate(HashSet<Diamon> res)
    {
        List<UniTask> uniTask = new();
        foreach (var diamon in res)
        {
            uniTask.Add(_visualMap[diamon].Scale(Vector3.zero, 0.2f));
        }
        await UniTask.WhenAll(uniTask);

        foreach (var diamon in res)
        {
            _visualMap[diamon].Deactivate();
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
                GridCell a = _diamondGrid.GetCell(new Vector2Int(i, j));
                if (!a.HasActivateDiamon())
                    continue;
                uniTask.Add(_visualMap[a.Diamond].Move(_diamondGrid.GridToWorld(a.GridPos), _swapTime));
            }
        }

        await UniTask.WhenAll(uniTask);
    }
}
