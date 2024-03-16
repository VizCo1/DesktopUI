using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsDataSO", menuName = "ScriptableObjects/SettingsDataSO")]
public class SettingsDataSO : ScriptableObject, IOrderedInitialization
{
    // Resolutions
    private List<string> _resolutionsList;
    private List<Resolution> _filteredResolutions;
    private Resolution _currentResolution;

    // Display modes
    private List<string> _displayModesList;
    private FullScreenMode _fullscreenMode;

    // Frame rates
    private List<int> _frameRatesList;
    private int _currentFrameRate;

    // VSync
    private bool _isVSync;

    // Volume UI
    private float _volumeUI;

    // Resolution properties
    public List<string> ResolutionsList { get => _resolutionsList; set => _resolutionsList = value; }
    public List<Resolution> FilteredResolutions { get => _filteredResolutions; set => _filteredResolutions = value; }
    public Resolution CurrentResolution { get=> _currentResolution; set => _currentResolution = value; }

    // Display mode properties
    public List<string> DisplayModesList { get => _displayModesList; set => _displayModesList = value; }
    public FullScreenMode FullscreenMode { get => _fullscreenMode; set => _fullscreenMode = value; }

    // Frame rate properties
    public List<int> FrameRatesList { get => _frameRatesList; set => _frameRatesList = value; }
    public int CurrentFrameRate { get => _currentFrameRate; set => _currentFrameRate = value; }

    // VSync property
    public bool VSync { get => _isVSync; set => _isVSync = value; }

    // Volume property
    public float VolumeUI { get => _volumeUI; set => _volumeUI = value; }

    private void OnEnable()
    {
        // Events from Presenter
        SettingsEvents.ResolutionChanged += SettingsEvents_ResolutionChanged;
        SettingsEvents.DisplayModeChanged += SettingsEvents_DisplayModeChanged;
        SettingsEvents.FrameRateChanged += SettingsEvents_FrameRateChanged;
        SettingsEvents.VSyncChanged += SettingsEvents_VSyncChanged;
        SettingsEvents.VolumeUIChanged += SettingsEvents_VolumeUIChanged;       
    }

    private void OnDisable()
    {
        // Events from Presenter
        SettingsEvents.ResolutionChanged -= SettingsEvents_ResolutionChanged;
        SettingsEvents.DisplayModeChanged -= SettingsEvents_DisplayModeChanged;
        SettingsEvents.FrameRateChanged -= SettingsEvents_FrameRateChanged;
        SettingsEvents.VSyncChanged -= SettingsEvents_VSyncChanged;
        SettingsEvents.VolumeUIChanged -= SettingsEvents_VolumeUIChanged;
    }

    public void Initialize() // This is called in the Initializer Start()
    {
        LoadSettings();
        ApplySettings();        
    }

    public void InitializeSettings()
    {
        InitializeFilteredResolutions();
        InitializeDisplayModes();
        InitializeFrameRates();
        InitializeVSync();
        InitializeVolumeUI();
    }

    #region Load settings

    private void LoadSettings()
    {
        _currentResolution = PersistentSettingsManager.LoadResolution();
        _fullscreenMode = PersistentSettingsManager.LoadDisplayMode();
        _currentFrameRate = PersistentSettingsManager.LoadFrameRate();
        _isVSync = PersistentSettingsManager.LoadVSync();
        _volumeUI = PersistentSettingsManager.LoadSystemVolume();
    }

    #endregion

    #region Apply settings

    private void ApplySettings()
    {
        //ApplyResolutionAndScreenMode(); No need to apply the first time
        ApplyTargetFrameRate();
        ApplyVSync();
        ApplyVolumeUI();
    }

    private void ApplyResolutionAndScreenMode()
    {
        Screen.SetResolution(_currentResolution.width, _currentResolution.height, _fullscreenMode);
        DOVirtual.DelayedCall(0.2f, () => SettingsEvents.OnResolutionChanged?.Invoke());
    }

    private void ApplyTargetFrameRate()
    {
        Application.targetFrameRate = _currentFrameRate;
    }

    private void ApplyVSync()
    {
        QualitySettings.vSyncCount = _isVSync ? 1 : 0;
    }

    private void ApplyVolumeUI()
    {
        SoundsManager.Instance.VolumeUI = _volumeUI;
    }

    #endregion

    #region Events from the presenter

    private void SettingsEvents_ResolutionChanged(int index)
    {
        _currentResolution = _filteredResolutions[index];
        PersistentSettingsManager.SaveResolution(_currentResolution);
        //ApplyResolutionAndScreenMode();
    }

    private void SettingsEvents_DisplayModeChanged(int index)
    {
        _fullscreenMode = Enum.Parse<FullScreenMode>(_displayModesList[index]);
        PersistentSettingsManager.SaveDisplayMode(_fullscreenMode);
        ApplyResolutionAndScreenMode();
    }

    private void SettingsEvents_FrameRateChanged(int index)
    {
        _currentFrameRate = _frameRatesList[index];
        PersistentSettingsManager.SaveFrameRate(_currentFrameRate);
        ApplyTargetFrameRate();
    }

    private void SettingsEvents_VSyncChanged(bool value)
    {
        _isVSync = value;
        PersistentSettingsManager.SaveVSync(_isVSync);
        ApplyVSync();
    }

    private void SettingsEvents_VolumeUIChanged(float value)
    {
        _volumeUI = value;
        PersistentSettingsManager.SaveSystemVolume(_volumeUI);
        ApplyVolumeUI();
    }

    #endregion

    #region Initialize settings

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

        SettingsEvents.ModelResolutionChanged?.Invoke(_resolutionsList, GetResolutionIndex(), _currentResolution);
    }

    private void InitializeDisplayModes()
    {
        _displayModesList = new List<string>
        {
            FullScreenMode.FullScreenWindow.ToString(),
            FullScreenMode.Windowed.ToString()
        };

        SettingsEvents.ModelDisplayModeChanged?.Invoke(_displayModesList, GetDisplayModeIndex(), _fullscreenMode);
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

        SettingsEvents.ModelFrameRateChanged?.Invoke(_frameRatesList, GetFrameRateIndex(), _currentFrameRate);
    }

    private void InitializeVSync()
    {
        SettingsEvents.ModelVSyncChanged?.Invoke(_isVSync);
    }

    private void InitializeVolumeUI()
    {
        SettingsEvents.ModelVolumeUIChanged?.Invoke(_volumeUI);
    }

    #endregion

    #region Get index

    public int GetResolutionIndex()
    {
        int selectedResolutionIndex = 0;
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            Resolution res = _filteredResolutions[i];
            if (res.width == _currentResolution.width && res.height == _currentResolution.height)
            {
                selectedResolutionIndex = i;
                break;
            }
        }

        return selectedResolutionIndex;
    }

    public int GetDisplayModeIndex()
    {
        int selectedDisplayModeIndex = 0;
        for (int i = 0; i < _displayModesList.Count; i++)
        {
            if (!Enum.TryParse(_displayModesList[i], out FullScreenMode mode))
            {
                Debug.LogError("Could not parse string in DisplayModesList to FullScreenMode enum");
            }

            if (mode == _fullscreenMode)
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
            if (frameRate == _currentFrameRate)
            {
                selectedFrameRateIndex = i;
                break;
            }
        }

        return selectedFrameRateIndex;
    }

    #endregion
}