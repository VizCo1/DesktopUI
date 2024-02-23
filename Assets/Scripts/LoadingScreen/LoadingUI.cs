using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{

    [SerializeField] private float _loadingAddValue = 0.01f;
    [SerializeField] private float _smallLoadingAddValue = 0.05f;
    [SerializeField] private float _loadingThreshold = 0.9f;
    
    [SerializeField] private Slider _loadingSlider;

    private void Start()
    {
        _loadingSlider.value = _loadingSlider.minValue;
    }

    public void HandleLoadingBar(bool isLoadDone)
    {
        if (isLoadDone)
        {
            _loadingSlider.value += _loadingAddValue;
        }
        else if (_loadingSlider.value < _loadingThreshold)
        {
            _loadingSlider.value += _smallLoadingAddValue;
        }
    }

    public bool IsLoadingBarCompleted() => _loadingSlider.value >= _loadingSlider.maxValue;
}
