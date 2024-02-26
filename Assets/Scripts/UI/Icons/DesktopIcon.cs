using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DesktopIcon : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] private DesktopUI _desktopUI;

    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _iconText;

    private RectTransform _rectTransform;
    private Vector2 _prevPos;
    int _clicksToOpen = 2;
    int _clicks = 0;

    public int ApplicationID { get; private set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(Vector2 pos, int id, ApplicationIcon minigameIcon)
    {
        ApplicationID = id;
        _iconImage.sprite = minigameIcon.icon;
        _iconText.SetText(minigameIcon.name);
        transform.position = pos;
        gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / ComputerController.Instance.GetMainCanvas().scaleFactor;
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
                ComputerController.Instance.HandleDesktopIconClicked(this);
                SoundsManager.Instance.PlayUISound();
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
