using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    private List<Resolution> _filteredResolutions;
    private List<string> _resolutionsOptions;

    public static SettingsController Instance { get; private set; }

    public event Action OnResolutionChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeFilteredResolutions();
    }

    private void InitializeFilteredResolutions()
    {
        _filteredResolutions = new List<Resolution>();

        _resolutionsOptions = new List<string>();
        RefreshRate currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        foreach (Resolution res in Screen.resolutions)
        {
            if (currentRefreshRate.value == res.refreshRateRatio.value)
            {
                _filteredResolutions.Add(res);
                string resString = $"{res.width} x {res.height}";
                _resolutionsOptions.Add(resString);
            }
        }
    }

    public List<string> GetResolutionOptions() => _resolutionsOptions;

    public int GetResolutionIndex()
    {
        int selectedResolutionIndex = -1;
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            Resolution res = _filteredResolutions[i];
            if (res.width == Screen.width && res.height == Screen.height)
            {
                selectedResolutionIndex = i;
                break;
            }
        }

        return selectedResolutionIndex;
    }

    public void ChangeResolution(int index)
    {
        Screen.SetResolution(_filteredResolutions[index].width, _filteredResolutions[index].height,
                FullScreenMode.FullScreenWindow, _filteredResolutions[index].refreshRateRatio);

        DOVirtual.DelayedCall(0.1f, () => OnResolutionChanged?.Invoke());
    }
}
