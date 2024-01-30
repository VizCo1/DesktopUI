using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

public class DesktopIcon : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] private DesktopUI _desktopUI;

    private RectTransform _rectTransform;
    private Vector2 _prevPos;

    private Vector2 _cursorPos;
    private int _clicksToOpen;

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
        //_clicksToOpen = 0;
        _rectTransform.anchoredPosition += eventData.delta / ComputerControllerUI.Instance.GetMainCanvas().scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_desktopUI.IsClosestPositionFree(_rectTransform.position))
        {
            FixIconPosition(_desktopUI.FindProperPosition(_rectTransform.position));
            _desktopUI.SwapIconPositionStatus(gameObject, _prevPos);
        }
        else
        {
            FixIconPosition(_desktopUI.FindProperPosition(_prevPos));
        }
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

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check prev position and actual position
        // Check how many seconds have passed from the last click
        //if (++_clicksToOpen == 2)
        //{
        //    _clicksToOpen = 0;
        //    ComputerControllerUI.Instance.HandleDesktopIconClicked(this);
        //}
        ComputerControllerUI.Instance.HandleDesktopIconClicked(this);
    }
}
