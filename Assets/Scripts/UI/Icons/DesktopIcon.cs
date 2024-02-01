using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesktopIcon : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] private DesktopUI _desktopUI;

    private RectTransform _rectTransform;
    private Vector2 _prevPos;
    int _clicksToOpen = 2;
    int _clicks = 0;

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
        if (_prevPos == new Vector2(transform.position.x, transform.position.y))
        {
            if (++_clicks == _clicksToOpen)
            {
                ComputerControllerUI.Instance.HandleDesktopIconClicked(this);
                _clicks = 0;
            }

            float delay = 0.5f;
            DOVirtual.DelayedCall(delay, () => _clicks = 0);
        }
        else
        {
            _clicks = 0;
        }
    }
}
