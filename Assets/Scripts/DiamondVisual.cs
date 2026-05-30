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

    public async UniTask Scale(Vector3 to, float time, Ease ease = Ease.InBack)
    {
        await _transform.DOScale(to, time).SetEase(ease).ToUniTask();
    }

    public void Activate()
    {
        Scale(Vector3.one, 0f).Forget();
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
