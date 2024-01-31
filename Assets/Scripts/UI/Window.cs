using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using DG.Tweening;

public class Window : MonoBehaviour
{

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _minimizeButton;
    [SerializeField] private MMF_Player _closingFeedbacks;
    [SerializeField] private MMF_Player _minimizingFeedbacks;
    
    private void Start()
    {
        _closeButton.onClick.AddListener(() => 
        {
            Close();
        });
        _minimizeButton.onClick.AddListener(() => 
        {
            Minimize();          
        });
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveAllListeners();
        _minimizeButton.onClick.RemoveAllListeners();
    }

    public void Open()
    {
        transform.SetAsLastSibling();
        _minimizingFeedbacks.Direction = MMFeedbacks.Directions.BottomToTop;
        _minimizingFeedbacks.PlayFeedbacks();
    }

    private void Close()
    {
        _closingFeedbacks.PlayFeedbacks();
        ComputerControllerUI.Instance.CloseWindowEffects();
        DOVirtual.DelayedCall(_closingFeedbacks.TotalDuration + 0.1f, () => ComputerControllerUI.Instance.CloseWindow(this));
    }

    public void Minimize()
    {
        ComputerControllerUI.Instance.SetIsDesktopState();
        _minimizingFeedbacks.Direction = MMFeedbacks.Directions.TopToBottom;
        _minimizingFeedbacks.PlayFeedbacks();
    }

    public void ToDefault()
    {
        gameObject.SetActive(false);
        GetComponent<CanvasGroup>().alpha = 1f;
    }
}
