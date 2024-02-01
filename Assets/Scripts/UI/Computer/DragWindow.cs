using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform _windowRectTransform;

    public void OnDrag(PointerEventData eventData)
    {
        _windowRectTransform.anchoredPosition += eventData.delta / ComputerControllerUI.Instance.GetMainCanvas().scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _windowRectTransform.SetAsLastSibling();
        Cursor.lockState = CursorLockMode.Confined;
    }
}
