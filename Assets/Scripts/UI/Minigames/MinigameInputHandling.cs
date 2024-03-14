using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinigameInputHandling : MonoBehaviour
{

    private const int RENDER_TEXTURE_WIDTH = 1920;
    private const int RENDER_TEXTURE_HEIGHT = 1000;

    //[Tooltip("The ID is the number minus 1 (Minigame1 --> ID = 0)")]
    //[SerializeField] private int _minigameID = -1;

    private GraphicRaycaster _graphicRaycaster;
    private EventSystem _eventSystem;

    private RectTransform _rawImageRectTransform;

    private void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _eventSystem = FindFirstObjectByType<EventSystem>();
    }

    private void Start()
    {
        // Get next rawImage rectTransform
        _rawImageRectTransform = MinigameInputHandlingHelper.GetNextRawImageRectTransform();
    }

    private void OnDestroy()
    {
        _rawImageRectTransform = null;
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        if (_rawImageRectTransform == null && !RectTransformUtility.RectangleContainsScreenPoint(_rawImageRectTransform, mousePos))
        {
            return;
        }
                
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rawImageRectTransform, mousePos, null, out Vector2 localPoint))
        {
            float rawImageHeight = _rawImageRectTransform.rect.height;
            float rawImageWidth = _rawImageRectTransform.rect.width;

            // Convert the local point to a point in the render texture
            mousePos.x = (localPoint.x / rawImageWidth + 0.5f) * RENDER_TEXTURE_WIDTH;
            mousePos.y = (localPoint.y / rawImageHeight + 0.5f) * RENDER_TEXTURE_HEIGHT;
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
