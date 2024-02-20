using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SettingsPresenter : MonoBehaviour
{
    private SettingsDataSO _settingsDataSO;

    private void Start()
    {
        Initialize();

        // Events from View
        SettingsEvents.ResolutionDropdownChanged += SettingsEvents_ResolutionDropdownChanged;
        SettingsEvents.DisplayModeDropdownChanged += SettingsEvents_DisplayModeDropdownChanged;
        SettingsEvents.FrameRateDropdownChanged += SettingsEvents_FrameRateDropdownChanged;
        SettingsEvents.VSyncToggleChanged += SettingsEvents_VSyncToggleChanged;
    }

    private void OnDestroy()
    {
        // Events from View
        SettingsEvents.ResolutionDropdownChanged -= SettingsEvents_ResolutionDropdownChanged;
        SettingsEvents.DisplayModeDropdownChanged -= SettingsEvents_DisplayModeDropdownChanged;
        SettingsEvents.FrameRateDropdownChanged -= SettingsEvents_FrameRateDropdownChanged;
        SettingsEvents.VSyncToggleChanged -= SettingsEvents_VSyncToggleChanged;
    }

    private void Initialize()
    {
        // Default values
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        _settingsDataSO = Resources.Load<SettingsDataSO>("SettingsDataSO");

        SettingsEvents.ResolutionSet?.Invoke(_settingsDataSO.ResolutionsList, _settingsDataSO.GetResolutionIndex());
        SettingsEvents.DisplayModeSet?.Invoke(_settingsDataSO.DisplayModesList, _settingsDataSO.GetDisplayModeIndex());
        SettingsEvents.FrameRateSet?.Invoke(_settingsDataSO.FrameRatesList, _settingsDataSO.GetFrameRateIndex());
        SettingsEvents.VSyncSet?.Invoke(_settingsDataSO.VSync);
    }

    private void SettingsEvents_ResolutionDropdownChanged(int index)
    {
        //SettingsEvents.ResolutionChanged?.Invoke(index);
        StartCoroutine(ChangeResolution(index));
    }

    private void SettingsEvents_DisplayModeDropdownChanged(int index)
    {
        SettingsEvents.DisplayModeChanged?.Invoke(index);
        ChangeDisplayMode();
    }

    private void SettingsEvents_FrameRateDropdownChanged(int index)
    {
        Application.targetFrameRate = _settingsDataSO.FrameRatesList[index];
    }

    private void SettingsEvents_VSyncToggleChanged(int value)
    {
        QualitySettings.vSyncCount = value;

        SettingsEvents.VSyncChanged?.Invoke(value == 1);
    }

    private IEnumerator ChangeResolution(int index)
    {
        Resolution newResolution = _settingsDataSO.FilteredResolutions[index];

        if (newResolution.width != Screen.currentResolution.width && newResolution.height != Screen.currentResolution.height)
        {
            Screen.SetResolution(newResolution.width, newResolution.height,
                    _settingsDataSO.FullscreenMode, newResolution.refreshRateRatio);

            yield return new WaitForSeconds(0.2f);

            SettingsEvents.OnResolutionChanged?.Invoke();
        }

        yield return null;
    }

    private void ChangeDisplayMode()
    {
        Screen.fullScreenMode = _settingsDataSO.FullscreenMode;
    }
}
