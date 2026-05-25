using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DiamondVisual : MonoBehaviour
{
    private DiamonSO _data;
    private SpriteRenderer _spriteRenderer;
    private Transform _transform;

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
}
