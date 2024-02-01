using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarIcon : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private BarUI _barUI;
    [SerializeField] private GameObject _selectedGameObject;
    [SerializeField] private MMF_Player _clickedFeedbacks;
    [SerializeField] private MMF_Player _appearedFeedbacks;

    private Button _icon;
    private RectTransform _rectTransform;

    public int Index { get; set; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _icon = GetComponent<Button>();

        SetSelectedVisuals(false);
    }

    private void Start()
    {
        _icon.onClick.AddListener(() =>
        {
            if (_barUI.GetPosition(Index) == new Vector2(transform.position.x, transform.position.y))
            {
                ComputerControllerUI.Instance.HandleBarIconClicked(this); 
                PlayClickedFeedbacks();
            }
        });
    }

    private void OnDestroy()
    {
        _icon.onClick.RemoveAllListeners();
    }

    public void Init(int index)
    {
        Index = index;
        transform.position = _barUI.GetPosition(Index);
        gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += new Vector2(eventData.delta.x / ComputerControllerUI.Instance.GetMainCanvas().scaleFactor, 0);     
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _barUI.IsMovingIcon = false;
        FixIconPosition(_barUI.GetPosition(Index));
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
        // Swap indices
        (movingIcon.Index, Index) = (Index, movingIcon.Index);

        // Fix position
        this.FixIconPosition(_barUI.GetPosition(Index));
    }

    public void FixIconPosition(Vector2 pos)
    {
        transform.DOMove(pos, 0.25f);
    }

    public void PlayClickedFeedbacks()
    {
        _clickedFeedbacks.PlayFeedbacks();
    }

    public void PlayAppearedFeedbacks(MMFeedbacks.Directions direction)
    {
        _appearedFeedbacks.Direction = direction;
        _appearedFeedbacks.PlayFeedbacks();
    }

    public void PlayAppearedFeedbacksAndDisable(MMFeedbacks.Directions direction)
    {
        PlayAppearedFeedbacks(direction);
        DOVirtual.DelayedCall(_appearedFeedbacks.TotalDuration + 0.1f, () => gameObject.SetActive(false));
    }

    public void SetSelectedVisuals(bool setter)
    {
        float endValue;
        if (setter)
        {
            endValue = 1;
            _selectedGameObject.transform.localScale = Vector3.zero;
        }
        else
        {
            endValue= 0;
            _selectedGameObject.transform.localScale = Vector3.one;
        }

        float duration = 0.25f;
        _selectedGameObject.transform.DOScale(endValue, duration)
            .SetEase(Ease.Linear);
    }
}
