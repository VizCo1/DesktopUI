using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

public class DesktopIcon : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    private RectTransform _rectTransform;
    private static bool _canBePlaced = true;
    private Vector2 _initialPos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(Vector2 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Desktop.Instance.IsMovingIcon = false;
        if (_canBePlaced)
        {
            FixIconPosition(Desktop.Instance.FindProperPosition(_rectTransform.position));
            Desktop.Instance.SetIconPositionStatus(gameObject, true);
            Desktop.Instance.SetIconPositionStatus(_initialPos, false);
        }
        else
        {
            FixIconPosition(Desktop.Instance.FindProperPosition(_initialPos));
        }

        _canBePlaced = true;
    }

    [Button]
    public void Remove()
    {
        Desktop.Instance.RemoveIcon(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _initialPos = transform.position;
        Desktop.Instance.IsMovingIcon = true;
        Desktop.Instance.SetMovingIcon(this);
        transform.SetAsFirstSibling();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Desktop.Instance.IsMovingIcon && this != Desktop.Instance.GetMovingIcon())
        {
            _canBePlaced = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Desktop.Instance.IsMovingIcon && this != Desktop.Instance.GetMovingIcon())
        {
            _canBePlaced = true;
        }
    }

    public void FixIconPosition(Vector2 pos)
    {
        transform.DOMove(pos, 0.25f);
    }
}
