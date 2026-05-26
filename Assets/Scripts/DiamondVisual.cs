using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DiamondVisual : MonoBehaviour, IPoolable
{
    private DiamonSO _data;
    private SpriteRenderer _spriteRenderer;
    private Transform _transform;

    public bool IsActive => gameObject.activeSelf;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
    }

    public void Visualize(DiamonSO data, Vector3 position)
    {
        _data = data;
        _spriteRenderer.sprite = data.Sprite;
        _transform.position = position;
    }

    public UniTask Move(Vector3 worldPos, float time)
    {
        return _transform.DOMove(worldPos, time).ToUniTask();
    }

    public async UniTask playdisapearanim()
    {
        await _transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).ToUniTask();
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
