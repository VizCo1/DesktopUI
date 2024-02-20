using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsDataSO", menuName = "ScriptableObjects/SettingsDataSO")]
public class SettingsDataSO : ScriptableObject
{

    // Resolutions
    private List<string> _resolutionsList;
    private List<Resolution> _filteredResolutions;

    // Display modes
    private List<string> _displayModesList;
    private FullScreenMode _fullscreenMode;

    // Frame rates
    private List<int> _frameRatesList;

    // VSync
    private bool _isVSync;

    // Properties
    public List<string> ResolutionsList { get => _resolutionsList; set => _resolutionsList = value; }
    public List<Resolution> FilteredResolutions { get => _filteredResolutions; set => _filteredResolutions = value; }
    public FullScreenMode FullscreenMode { get => _fullscreenMode; set => _fullscreenMode = value; }
    public List<string> DisplayModesList { get => _displayModesList; set => _displayModesList = value; }
    public List<int> FrameRatesList { get => _frameRatesList; set => _frameRatesList = value; }
    public bool VSync { get => _isVSync; set => _isVSync = value; }

    private void OnEnable()
    {
        InitializeFilteredResolutions();
        InitializeDisplayModes();
        InitializeFrameRates();
        InitializeVSync();

        // Events from Presenter
        SettingsEvents.DisplayModeChanged += SettingsEvents_DisplayModeChanged;
        SettingsEvents.VSyncChanged += SettingsEvents_VSyncChanged;
    }

    private void SettingsEvents_DisplayModeChanged(int index)
    {
        _fullscreenMode = Enum.Parse<FullScreenMode>(_displayModesList[index]);
    }

    private void SettingsEvents_VSyncChanged(bool value)
    {
        _isVSync = value;
    }

    private void InitializeFilteredResolutions()
    {
        _resolutionsList = new List<string>();
        _filteredResolutions = new List<Resolution>();

        RefreshRate currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        foreach (Resolution res in Screen.resolutions)
        {
            if (currentRefreshRate.value == res.refreshRateRatio.value)
            {
                _filteredResolutions.Add(res);
                string resString = $"{res.width} x {res.height}";
                _resolutionsList.Add(resString);
            }
        }
    }

    private void InitializeDisplayModes()
    {
        _fullscreenMode = Screen.fullScreenMode; // Will be refactored!!
        _displayModesList = new List<string>
        {
            FullScreenMode.FullScreenWindow.ToString(),
            FullScreenMode.Windowed.ToString()
        };
    }

    private void InitializeFrameRates()
    {
        _frameRatesList = new List<int>
        {
            30,             
            60,
            90,
            120,
        };
    }

    private void InitializeVSync()
    {
        if (QualitySettings.vSyncCount == 1)
        {
            _isVSync = true;
        }
        else
        {
            _isVSync = false;
        }
    }

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

    public int GetDisplayModeIndex()
    {
        int selectedDisplayModeIndex = -1;
        for (int i = 0; i < _displayModesList.Count; i++)
        {
            if (!Enum.TryParse(_displayModesList[i], out FullScreenMode mode))
            {
                Debug.LogError("Could not parse string in DisplayModesList to FullScreenMode enum");
            }

            if (mode == Screen.fullScreenMode)
            {
                selectedDisplayModeIndex = i;
                break;
            }
        }

        return selectedDisplayModeIndex;
    }

    public int GetFrameRateIndex()
    {
        int selectedFrameRateIndex = 0;
        for (int i = 0; i < _frameRatesList.Count; i++)
        {
            int frameRate = _frameRatesList[i];
            if (frameRate == Application.targetFrameRate)
            {
                selectedFrameRateIndex = i;
                break;
            }
        }

        return selectedFrameRateIndex;
    }
}
