using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinigameButton : MonoBehaviour
{
    [Header("Color transitions")]
    [SerializeField] private Color _onHoverColor;
    [SerializeField] private Color _onClickDownColor;
    [SerializeField] private Color _normalColor;
    [SerializeField] private float _fadeDuration = 0.1f;

    [Header("Sound")]
    [SerializeField] private bool _hasSound;

    private Image _image;
    private bool _clickedDown;
    private Tween _exitMouseTween;

    public event Action OnClicked;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        if (_hasSound)
        {
            OnClicked += PlaySound;
        }
    }

    private void OnDestroy()
    {
        if (_hasSound)
        {
            OnClicked -= PlaySound;
        }
    }

    private void PlaySound()
    {
        SoundsManager.Instance.PlayUISound();
    }

    #region Mouse events

    public void OnLeftClickDown()
    {
        _image.DOColor(_onClickDownColor, _fadeDuration);
        _clickedDown = true;
    }

    public void OnLeftClickUp()
    {
        _image.DOColor(_normalColor, _fadeDuration);
        if (_clickedDown)
        {
            _clickedDown = false;
            OnClicked?.Invoke();
        }
    }

    public void OnHover()
    {
        _exitMouseTween.Kill();
        _image.DOColor(_onHoverColor, _fadeDuration);
        _exitMouseTween = DOVirtual.DelayedCall(0.1f, () => MouseExit())
            .Play();
    }

    private void MouseExit()
    {
        _clickedDown = false;
        _image.DOColor(_normalColor, _fadeDuration);
    }

    #endregion
}
