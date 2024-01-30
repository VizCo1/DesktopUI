using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BarIcon : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private BarUI _barUI;

    private Button _icon;
    private RectTransform _rectTransform;
    private Vector2 _iconPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _icon = GetComponent<Button>();
    }

    private void Start()
    {
        _icon.onClick.AddListener(() => ComputerControllerUI.Instance.HandleBarIconClicked(this));
    }

    private void OnDestroy()
    {
        _icon.onClick.RemoveAllListeners();
    }

    public void Init(Vector2 pos)
    {
        _iconPos = pos;
        transform.position = pos;
        gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += new Vector2(eventData.delta.x / ComputerControllerUI.Instance.GetMainCanvas().scaleFactor, 0);     
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _barUI.IsMovingIcon = false;
        FixIconPosition(_iconPos);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_barUI.IsMovingIcon && this != _barUI.GetMovingIcon())
        {
            SwapPosWith(_barUI.GetMovingIcon());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _barUI.IsMovingIcon = true;
        _barUI.SetMovingIcon(this);
        transform.SetAsFirstSibling();
    }

    public void SwapPosWith(BarIcon movingIcon)
    {
        Vector2 tempIconPos = _iconPos;
        _iconPos = movingIcon.GetIconPos();
        movingIcon.SetIconPos(tempIconPos);
        FixIconPosition(_iconPos);
    }

    public void FixIconPosition(Vector2 pos)
    {
        transform.DOMove(pos, 0.25f);
    }

    public Vector2 GetIconPos() => _iconPos;
    public void SetIconPos(Vector2 pos) => _iconPos = pos;
}
