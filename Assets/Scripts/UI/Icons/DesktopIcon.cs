using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

public class DesktopIcon : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform _rectTransform;
    private Vector2 _prevPos;

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
        _rectTransform.anchoredPosition += eventData.delta / GameController.Instance.GetMainCanvas().scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (DesktopUI.Instance.IsClosestPositionFree(_rectTransform.position))
        {
            FixIconPosition(DesktopUI.Instance.FindProperPosition(_rectTransform.position));
            DesktopUI.Instance.SwapIconPositionStatus(gameObject, _prevPos);
        }
        else
        {
            FixIconPosition(DesktopUI.Instance.FindProperPosition(_prevPos));
        }
    }

    [Button]
    public void Remove()
    {
        DesktopUI.Instance.RemoveIcon(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _prevPos = transform.position;
        transform.SetAsFirstSibling();
    }

    public void FixIconPosition(Vector2 pos)
    {
        transform.DOMove(pos, 0.25f);
    }
}
