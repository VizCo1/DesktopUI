using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BarIcon : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerDownHandler
{
    private RectTransform _rectTransform;
    private Vector2 _iconPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(Vector2 pos)
    {
        _iconPos = pos;
        transform.position = pos;
        gameObject.SetActive(true);
    }


    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += new Vector2(eventData.delta.x / GameController.Instance.GetMainCanvas().scaleFactor, 0);     
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        BottomBarUI.Instance.IsMovingIcon = false;
        FixIconPosition(transform, _iconPos);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BottomBarUI.Instance.IsMovingIcon && this != BottomBarUI.Instance.GetMovingIcon())
        {
            SwapPosWith(BottomBarUI.Instance.GetMovingIcon());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BottomBarUI.Instance.IsMovingIcon = true;
        BottomBarUI.Instance.SetMovingIcon(this);
        transform.SetAsFirstSibling();
    }

    public void SwapPosWith(BarIcon movingIcon)
    {
        Vector2 tempIconPos = _iconPos;
        _iconPos = movingIcon.GetIconPos();
        movingIcon.SetIconPos(tempIconPos);
        FixIconPosition(transform, _iconPos);
    }

    public void FixIconPosition(Transform icon, Vector2 pos)
    {
        icon.DOMove(pos, 0.25f);
    }

    public Vector2 GetIconPos() => _iconPos;
    public void SetIconPos(Vector2 pos) => _iconPos = pos;
}
