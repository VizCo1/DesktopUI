using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsWindow : Window
{
    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown _resolutionsDropdown;
    [SerializeField] private TMP_Dropdown _displayModeDropdown;
    [SerializeField] private TMP_Dropdown _frameRateCapDropdown;

    [Header("Toggles")]
    [SerializeField] private Toggle _vsyncToggle;

    protected override void OnEnable()
    {
        base.OnEnable();

        SubcribeToEvents();
        RegisterCallbacks();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
        UnRegisterCallbacks();
    }

    protected override void Start()
    {
        base.Start();

        _frameRateCapDropdown.interactable = !_vsyncToggle.isOn;
    }

    private void SubcribeToEvents()
    {
        // Events from presenter
        SettingsEvents.ResolutionSet += InitResolutionsDropdown;
        SettingsEvents.DisplayModeSet += InitDisplayModeDropdown;
        SettingsEvents.FrameRateSet += InitFrameRateCapDropdown;
        SettingsEvents.VSyncSet += InitVSyncToggle;
    }

    private void UnsubscribeFromEvents()
    {
        // Events from presenter
        SettingsEvents.ResolutionSet -= InitResolutionsDropdown;
        SettingsEvents.DisplayModeSet -= InitDisplayModeDropdown;
        SettingsEvents.FrameRateSet -= InitFrameRateCapDropdown;
        SettingsEvents.VSyncSet -= InitVSyncToggle;
    }

    private void RegisterCallbacks()
    {
        _resolutionsDropdown.onValueChanged.AddListener((int index) =>
        {
            SettingsEvents.ResolutionDropdownChanged?.Invoke(index);
        });

        _displayModeDropdown.onValueChanged.AddListener((int index) =>
        {
            SettingsEvents.DisplayModeDropdownChanged?.Invoke(index);
        });

        _frameRateCapDropdown.onValueChanged.AddListener((int index) =>
        {
            SettingsEvents.FrameRateDropdownChanged?.Invoke(index);
        });

        _vsyncToggle.onValueChanged.AddListener((bool value) =>
        {
            SettingsEvents.VSyncToggleChanged?.Invoke( value ? 1 : 0);

            _frameRateCapDropdown.interactable = !value;
        });
    }

    private void UnRegisterCallbacks()
    {
        _resolutionsDropdown.onValueChanged.RemoveAllListeners();
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
}
