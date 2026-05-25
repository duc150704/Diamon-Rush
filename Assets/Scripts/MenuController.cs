using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;


public class MenuController : MonoBehaviour
{
    [SerializeField] private UIButton _play;
    [SerializeField] private UIButton _music;
    [SerializeField] private UIButton _sound;
    [SerializeField] private UIButton _setting;
    [SerializeField] private UIPanel _name;

    [SerializeField] private Sprite _circleButtonBG_ON;
    [SerializeField] private Sprite _circleButtonBG_OFF;

    [SerializeField] private float _slideDuration = 0.5f;

    private List<UIButton> _uiButton = new();

    private void Start()
    {
        ListingButton();
        HandleEvent();
        AnimateStartUI();
    }

    private void ListingButton()
    {
        _uiButton.Add(_play);
        _uiButton.Add(_music);
        _uiButton.Add(_sound);
        _uiButton.Add(_setting);
    }

    private void HandleEvent()
    {
        _play.AddListener(HandlePlayButton);
        _music.AddListener(HandleMusicButton);
        _sound.AddListener(HandleSoundButton);
        _setting.AddListener(HandleSettingButton);
    }

    public void PlayButtonSound()
        => SoundController.Instance.Play(ESoundType.ButtonClickSound);

    public void HandleSettingButton()
    {
        PlayButtonSound();
        AnimateTounchButton(_setting);
    }

    public void AnimateTounchButton(UIButton button, Action callBack = null)
    {
        button.SetInteractable(false);
        Sequence sequence = DOTween.Sequence();
        Vector3 originalScale = button.LocalScale;

        sequence.Append(button.Scale(button.Rect, originalScale, originalScale * 0.8f, 0.1f));
        sequence.Append(button.Scale(button.Rect, button.LocalScale, originalScale, 0.1f))
            .OnComplete(() =>
            {
                button.SetInteractable(true);
                callBack?.Invoke();
            });
    }

    public void HandlePlayButton()
    {
        PlayButtonSound();
        AnimateTounchButton(_play, async () =>
        {
            SetAllButtonInteractable(false);
            await SceneController.Instance.Load(EScene.Play);
        });
    }

    public void HandleMusicButton()
    {
        PlayButtonSound();
        AnimateTounchButton(_music);
        bool isMuted = SoundController.Instance.ToggleMusic();
        ChangeButtonBG(_music, (isMuted) ? _circleButtonBG_OFF : _circleButtonBG_ON);
    }

    public void HandleSoundButton()
    {
        PlayButtonSound();
        AnimateTounchButton(_sound);
        bool isMuted = SoundController.Instance.ToggleSound();
        ChangeButtonBG(_sound, (isMuted) ? _circleButtonBG_OFF : _circleButtonBG_ON);
    }

    private void ChangeButtonBG(UIButton button, Sprite sprite)
    {
        button.SetSprite(sprite);
    }

    private void AnimateStartUI()
    {
        _play.Slide(_play.Rect,_play.AnPos + new Vector3(0, -550), _play.AnPos, _slideDuration, Ease.OutBack);
        _music.Slide(_music.Rect, _music.AnPos + new Vector3(0, 450), _music.AnPos, _slideDuration, Ease.OutBack);
        _sound.Slide(_sound.Rect, _sound.AnPos + new Vector3(0, 450), _sound.AnPos, _slideDuration, Ease.OutBack);
        _setting.Slide(_setting.Rect, _setting.AnPos + new Vector3(0, 450), _setting.AnPos, _slideDuration, Ease.OutBack);
        _name.Scale(_name.Rect, new Vector2(0, 0), _name.Rect.localScale, _slideDuration, Ease.OutBack);
        SoundController.Instance.Play(ESoundType.BackgroundMusic);
    }

    private void SetAllButtonInteractable(bool value)
    {
        foreach (var item in _uiButton)
        {
            item.SetInteractable(value);
        }
    }
}
