using DG.Tweening;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    public RectTransform Rect {  get; private set; }
    public Vector3 LocalScale => Rect.localScale;
    public Vector3 AnPos => Rect.anchoredPosition;

    protected virtual void Awake()
    {
        Rect = GetComponent<RectTransform>();
    }

    // de co the thao tac duoc voi obj con
    public Tween Slide(RectTransform target, Vector3 from, Vector3 to, float duration, Ease ease = Ease.Linear)
    {
        target.anchoredPosition = from;
        return target.DOAnchorPos(to, duration).SetEase(ease);
    }

    public Tween Scale(RectTransform target, Vector3 from, Vector3 to, float duration, Ease ease = Ease.Linear)
    {
        target.localScale = from;
        return target.DOScale(to, duration).SetEase(ease);
    }

    public Tween Rotate(RectTransform target, float angle, float duration)
    {
        return target.DOLocalRotate(new Vector3(0f,0f, angle), duration, RotateMode.FastBeyond360);
    }
}
