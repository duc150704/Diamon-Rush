using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class UIButton : UIObject
{
    protected Button _button;

    [SerializeField] protected Image _background;
    [SerializeField] protected Image _icon;
    [SerializeField] protected TextMeshProUGUI _text;

    protected override void Awake()
    {
        base.Awake();
        _button = GetComponent<Button>();
    }

    public void SetSprite(Sprite sprite)
    {
        _background.sprite = sprite;
    }

    public void SetInteractable(bool value)
    {
        _button.interactable = value;
    }

    public Image GetIcon()
    {
        return _icon;
    }

    public void AddListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }
}
