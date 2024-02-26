using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : Window
{
    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown _resolutionsDropdown;
    [SerializeField] private TMP_Dropdown _displayModeDropdown;
    [SerializeField] private TMP_Dropdown _frameRateCapDropdown;

    [Header("Toggles")]
    [SerializeField] private Toggle _vsyncToggle;
    
    [Header("Sliders")]
    [SerializeField] private Slider _systemVolumeSlider;

    [Header("Buttons")]
    [SerializeField] private Button _applySettingsButton;

    private void Awake()
    {
        SubcribeToEvents();
        RegisterCallbacks();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        UnsubscribeFromEvents();
        UnRegisterCallbacks();
    }

    protected override void Start()
    {
        base.Start();

        _frameRateCapDropdown.interactable = !_vsyncToggle.isOn;
        SoundsManager.Instance.VolumeUI = _systemVolumeSlider.value;
    }

    private void SubcribeToEvents()
    {
        // Events from presenter
        SettingsEvents.ResolutionSet += InitResolutionsDropdown;
        SettingsEvents.DisplayModeSet += InitDisplayModeDropdown;
        SettingsEvents.FrameRateSet += InitFrameRateCapDropdown;
        SettingsEvents.VSyncSet += InitVSyncToggle;
        SettingsEvents.VolumeUISet += InitSystemVolume;
    }

    private void UnsubscribeFromEvents()
    {
        // Events from presenter
        SettingsEvents.ResolutionSet -= InitResolutionsDropdown;
        SettingsEvents.DisplayModeSet -= InitDisplayModeDropdown;
        SettingsEvents.FrameRateSet -= InitFrameRateCapDropdown;
        SettingsEvents.VSyncSet -= InitVSyncToggle;
        SettingsEvents.VolumeUISet -= InitSystemVolume;
    }

    private void RegisterCallbacks()
    {
        _systemVolumeSlider.onValueChanged.AddListener((float value) =>
        {
            SoundsManager.Instance.VolumeUI = value;
        });

        _vsyncToggle.onValueChanged.AddListener((bool value) =>
        {
            _frameRateCapDropdown.interactable = !value;
        });

        _applySettingsButton.onClick.AddListener(() =>
        {
            SoundsManager.Instance.PlayConfirmSound();

            SettingsEvents.ResolutionDropdownChanged?.Invoke(_resolutionsDropdown.value);
            SettingsEvents.DisplayModeDropdownChanged?.Invoke(_displayModeDropdown.value);
            SettingsEvents.FrameRateDropdownChanged?.Invoke(_frameRateCapDropdown.value);
            SettingsEvents.VSyncToggleChanged?.Invoke(_vsyncToggle.isOn ? 1 : 0);
            SettingsEvents.VolumeUISliderChanged?.Invoke(_systemVolumeSlider.value);
        });
    }

    private void UnRegisterCallbacks()
    {
        _systemVolumeSlider.onValueChanged.RemoveAllListeners();
        _applySettingsButton.onClick.RemoveAllListeners();
        _vsyncToggle.onValueChanged.RemoveAllListeners();
    }

    private void InitResolutionsDropdown(List<string> resolutionsList, int index)
    {
        _resolutionsDropdown.ClearOptions();

        _resolutionsDropdown.AddOptions(resolutionsList);

        _resolutionsDropdown.value = index;
    }

    private void InitDisplayModeDropdown(List<string> displayModesList, int index)
    {
        _displayModeDropdown.ClearOptions();

        _displayModeDropdown.AddOptions(displayModesList);

        _displayModeDropdown.value = index;
    }

    private void InitFrameRateCapDropdown(List<int> frameRateList, int index)
    {
        _frameRateCapDropdown.ClearOptions();

        List<string> frameRateStringList = frameRateList.ConvertAll((int i) => { return i.ToString(); });

        _frameRateCapDropdown.AddOptions(frameRateStringList);

        _frameRateCapDropdown.value = index;
    }

    private void InitVSyncToggle(bool value)
    {
        _vsyncToggle.isOn = value;
    }

    private void InitSystemVolume(float volume)
    {
        _systemVolumeSlider.value = volume;
    }
}