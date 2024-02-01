using UnityEngine;
using UnityEngine.EventSystems;

public class RenderTextureRaycast : MonoBehaviour
{
    [SerializeField] protected Camera MainCamera;
    [SerializeField] protected RectTransform RawImageRectTrans;
    [SerializeField] protected Camera RenderToTextureCamera;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(RawImageRectTrans, eventData.position, MainCamera, out localPoint);
        Vector2 normalizedPoint = Rect.PointToNormalized(RawImageRectTrans.rect, localPoint);

        var renderRay = RenderToTextureCamera.ViewportPointToRay(normalizedPoint);
        if (Physics.Raycast(renderRay, out var raycastHit))
        {
            Debug.Log("Hit: " + raycastHit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("No hit object");
        }
    }
}
