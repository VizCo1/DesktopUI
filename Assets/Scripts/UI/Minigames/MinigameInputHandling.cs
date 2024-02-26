using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinigameInputHandling : MonoBehaviour
{
    private GraphicRaycaster _graphicRaycaster;
    private EventSystem _eventSystem;

    public static RectTransform _rawImageRectTransform;

    private void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _eventSystem = FindFirstObjectByType<EventSystem>();
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        float imageHeight = _rawImageRectTransform.rect.height;
        float imageWidth = _rawImageRectTransform.rect.width;

        if (RectTransformUtility.RectangleContainsScreenPoint(_rawImageRectTransform, mousePos))
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rawImageRectTransform, mousePos, null, out Vector2 localPoint))
            {
                mousePos.x = (localPoint.x / imageWidth + 0.5f) * 1920;
                mousePos.y = (localPoint.y / imageHeight + 0.5f) * 1000;

                Debug.Log(mousePos);
            }
        }

        PointerEventData pointerEventData = new PointerEventData(_eventSystem);
        pointerEventData.position = mousePos;
        
        List<RaycastResult> results = new List<RaycastResult>();

        _graphicRaycaster.Raycast(pointerEventData, results);
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.TryGetComponent(out MinigameButton minigameButton))
            {
                HandleMinigameButtonEvents(minigameButton);
            }
        }
    }

    private void HandleMinigameButtonEvents(MinigameButton minigameButton)
    {
        if (Input.GetMouseButtonDown(0))
        {
            minigameButton.OnLeftClickDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            minigameButton.OnLeftClickUp();
        }
        else
        {
            minigameButton.OnHover();
        }
    }
}
