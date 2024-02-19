using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsWindow : Window
{
    [Header("Settings")]
    [SerializeField] private TMP_Dropdown _resolutionsDropdown;

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

    private void SubcribeToEvents()
    {
        // Event from presenter
        SettingsEvents.ResolutionSet += InitResolutionsDropdown;
    }

    private void UnsubscribeFromEvents()
    {
        // Event from presenter
        SettingsEvents.ResolutionSet -= InitResolutionsDropdown;
    }

    private void RegisterCallbacks()
    {
        _resolutionsDropdown.onValueChanged.AddListener((int index) =>
        {
            SettingsEvents.ResolutionDropdownChanged?.Invoke(index);
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
}
