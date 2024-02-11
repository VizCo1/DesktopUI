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

    private List<Resolution> _filteredResolutions;

    [SerializeField] DesktopUI desktopUI;
    [SerializeField] BarUI barUI;

    protected override void Start()
    {
        base.Start();

        InitResolutionsDropdown();

        _resolutionsDropdown.onValueChanged.AddListener((int index) =>
        {
            SettingsController.Instance.ChangeResolution(index);
        });
    }

    private void InitResolutionsDropdown()
    {
        _resolutionsDropdown.ClearOptions();

        _resolutionsDropdown.AddOptions(SettingsController.Instance.GetResolutionOptions());

        _resolutionsDropdown.value = SettingsController.Instance.GetResolutionIndex();
    }
}
