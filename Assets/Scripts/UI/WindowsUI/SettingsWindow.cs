using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VInspector;

public class SettingsWindow : Window
{
    [Header("Settings")]
    [SerializeField] private TMP_Dropdown _resolutionsDropdown;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;

    public event Action OnResolutionChanged;

    [SerializeField] DesktopUI desktopUI;
    [SerializeField] BarUI barUI;

    protected override void Start()
    {
        base.Start();

        InitResolutionsDropdown();

        _resolutionsDropdown.onValueChanged.AddListener((int index) =>
        {
            
            Screen.SetResolution(_filteredResolutions[index].width, _filteredResolutions[index].height,
                FullScreenMode.FullScreenWindow, _filteredResolutions[index].refreshRateRatio);

            //OnResolutionChanged?.Invoke();

            DOVirtual.DelayedCall(0.2f, () =>
            {
                barUI.InitializeSpace();
                desktopUI.InitializeSpace();
                desktopUI.FixAllIcons();
                // Fill all desktop icon position!!!
            });
        });
    }

    [Button]
    public void Do()
    {
        barUI.InitializeSpace();
        desktopUI.InitializeSpace();

        desktopUI.FixAllIcons();
        barUI.FixAllIcons();
    }

    private void InitResolutionsDropdown()
    {
        _resolutionsDropdown.ClearOptions();

        _resolutions = Screen.resolutions;

        _filteredResolutions = new List<Resolution>();

        List<string> options = new List<string>();
        RefreshRate currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        foreach (Resolution res in _resolutions)
        {
            if (currentRefreshRate.value == res.refreshRateRatio.value)
            {
                _filteredResolutions.Add(res);
                string resString = $"{res.width} x {res.height}";
                options.Add(resString);
            }
        }

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

        _resolutionsDropdown.AddOptions(options);

        _resolutionsDropdown.value = selectedResolutionIndex;
    }
}
