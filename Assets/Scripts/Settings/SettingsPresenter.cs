using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPresenter : MonoBehaviour
{
    private SettingsDataSO _settingsDataSO;
    private bool _isInitialized;

    private void Awake()
    {
        // Events from View
        SettingsEvents.ResolutionDropdownChanged += SettingsEvents_ResolutionDropdownChanged;
        SettingsEvents.DisplayModeDropdownChanged += SettingsEvents_DisplayModeDropdownChanged;
        SettingsEvents.FrameRateDropdownChanged += SettingsEvents_FrameRateDropdownChanged;
        SettingsEvents.VSyncToggleChanged += SettingsEvents_VSyncToggleChanged;
        SettingsEvents.VolumeUISliderChanged += SettingsEvents_VolumeUISliderChanged;

        // Events from Model
        SettingsEvents.ModelResolutionChanged += SettingsEvents_ModelResolutionChanged;
        SettingsEvents.ModelDisplayModeChanged += SettingsEvents_ModelDisplayModeChanged;
        SettingsEvents.ModelFrameRateChanged += SettingsEvents_ModelFrameRateChanged;
        SettingsEvents.ModelVSyncChanged += SettingsEvents_ModelVSyncChanged;
        SettingsEvents.ModelVolumeUIChanged += SettingsEvents_ModelVolumeUIChanged;
    }

    private void OnEnable()
    {
        // This needs to happen to change the visuals of the UI
        if (_isInitialized)
        {
            _settingsDataSO.InitializeSettings();
        }
    }

    private void Start()
    {
        _settingsDataSO = Resources.Load<SettingsDataSO>("SettingsDataSO");
        _settingsDataSO.InitializeSettings();

        _isInitialized = true;     
    }

    private void OnDestroy()
    {
        // Events from View
        SettingsEvents.ResolutionDropdownChanged -= SettingsEvents_ResolutionDropdownChanged;
        SettingsEvents.DisplayModeDropdownChanged -= SettingsEvents_DisplayModeDropdownChanged;
        SettingsEvents.FrameRateDropdownChanged -= SettingsEvents_FrameRateDropdownChanged;
        SettingsEvents.VSyncToggleChanged -= SettingsEvents_VSyncToggleChanged;
        SettingsEvents.VolumeUISliderChanged -= SettingsEvents_VolumeUISliderChanged;

        // Events from Model
        SettingsEvents.ModelResolutionChanged -= SettingsEvents_ModelResolutionChanged;
        SettingsEvents.ModelDisplayModeChanged -= SettingsEvents_ModelDisplayModeChanged;
        SettingsEvents.ModelFrameRateChanged -= SettingsEvents_ModelFrameRateChanged;
        SettingsEvents.ModelVSyncChanged -= SettingsEvents_ModelVSyncChanged;
        SettingsEvents.ModelVolumeUIChanged -= SettingsEvents_ModelVolumeUIChanged;
    }

    #region Events from view

    private void SettingsEvents_ResolutionDropdownChanged(int index)
    {
        SettingsEvents.ResolutionChanged?.Invoke(index);
        //StartCoroutine(ChangeResolution(_settingsDataSO.FilteredResolutions[index]));
    }

    private void SettingsEvents_DisplayModeDropdownChanged(int index)
    {
        SettingsEvents.DisplayModeChanged?.Invoke(index);
        //FullScreenMode fullScreenMode = Enum.Parse<FullScreenMode>(_settingsDataSO.DisplayModesList[index]);
        //ChangeDisplayMode(fullScreenMode);
    }

    private void SettingsEvents_FrameRateDropdownChanged(int index)
    {
        SettingsEvents.FrameRateChanged?.Invoke(index);
    }

    private void SettingsEvents_VSyncToggleChanged(int value)
    {       
        SettingsEvents.VSyncChanged?.Invoke(value == 1);
    }

    private void SettingsEvents_VolumeUISliderChanged(float value)
    {
        SettingsEvents.VolumeUIChanged?.Invoke(value);
    }

    #endregion

    #region Events from model

    private void SettingsEvents_ModelResolutionChanged(List<string> resolutionList, int index, Resolution newResolution)
    {
        SettingsEvents.ResolutionSet?.Invoke(resolutionList, index);
    }

    private void SettingsEvents_ModelDisplayModeChanged(List<string> displayList, int index, FullScreenMode fullScreenMode)
    {
        SettingsEvents.DisplayModeSet?.Invoke(displayList, index);
    }

    private void SettingsEvents_ModelFrameRateChanged(List<int> frameRateList, int index, int frameRate)
    {
        SettingsEvents.FrameRateSet?.Invoke(frameRateList, index);
    }

    private void SettingsEvents_ModelVSyncChanged(bool isVSync)
    {
        SettingsEvents.VSyncSet?.Invoke(isVSync);
    }

    private void SettingsEvents_ModelVolumeUIChanged(float volume)
    {
        SettingsEvents.VolumeUISet?.Invoke(volume);
    }

    #endregion

    //private IEnumerator ChangeResolution(int index)
    //{
    //    Resolution newResolution = _settingsDataSO.FilteredResolutions[index];

    //    if (newResolution.width != Screen.currentResolution.width && newResolution.height != Screen.currentResolution.height)
    //    {
    //        Screen.SetResolution(newResolution.width, newResolution.height,
    //                _settingsDataSO.FullscreenMode, newResolution.refreshRateRatio);

    //        yield return new WaitForSeconds(0.2f);

    //        SettingsEvents.OnResolutionChanged?.Invoke();
    //    }

    //    yield return null;
    //}

    private IEnumerator ChangeResolution(Resolution newResolution)
    {
        if (newResolution.width != Screen.currentResolution.width && newResolution.height != Screen.currentResolution.height)
        {
            Screen.SetResolution(newResolution.width, newResolution.height,
                    _settingsDataSO.FullscreenMode, newResolution.refreshRateRatio);

            yield return new WaitForSeconds(0.2f);

            // Special event to inform the computer
            SettingsEvents.OnResolutionChanged?.Invoke();
        }

        yield return null;
    }

    private void ChangeDisplayMode(FullScreenMode fullScreenMode)
    {
        Screen.fullScreenMode = fullScreenMode;
    }

    private void ChangeVolumeUI(float volume)
    {
        SoundsManager.Instance.VolumeUI = volume;
    }

    private void ChangeFrameRate(int frameRate)
    {
        Application.targetFrameRate = frameRate;
    }

    private void ChangeVSync(int vSync)
    {
        QualitySettings.vSyncCount = vSync;
    }
}