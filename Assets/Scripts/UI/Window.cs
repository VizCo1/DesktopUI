using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class Window : MonoBehaviour
{

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _minimizeButton;
    [SerializeField] private MMF_Player _closingFeedbacks;
    [SerializeField] private MMF_Player _minimizingFeedbacks;

    //public event Action OnWindowOpened; // Called by the icon
    //public event Action OnWindowClosed; // Called by the close window button
    
    private void OnEnable()
    {
        _closeButton.onClick.AddListener(() => { _closingFeedbacks.PlayFeedbacks(); /*OnWindowClosed?.Invoke();*/ });
        _minimizeButton.onClick.AddListener(() => 
        {
            // Reuse this Feedback to minimize y maximize the window
            if (_minimizingFeedbacks.Direction == MMFeedbacks.Directions.TopToBottom)
            {
                _minimizingFeedbacks.Direction = MMFeedbacks.Directions.BottomToTop;
            }
            else
            {
                _minimizingFeedbacks.Direction = MMFeedbacks.Directions.TopToBottom;
            }

            _minimizingFeedbacks.PlayFeedbacks(); 
        });
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveAllListeners();
        _minimizeButton.onClick.RemoveAllListeners();
    }
}
