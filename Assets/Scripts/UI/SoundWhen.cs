using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundWhen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Serializable]
    private enum When
    {
        PointerUp,
        PointerDown,
    }

    [SerializeField] private When _when = When.PointerDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_when != When.PointerDown)
        {
            return;
        }

        if (eventData.button == 0)
        {
            SoundsManager.Instance.PlayUISound();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_when != When.PointerUp)
        {
            return;
        }

        if (eventData.button == 0)
        {
            SoundsManager.Instance.PlayUISound();
        }
    }
}
