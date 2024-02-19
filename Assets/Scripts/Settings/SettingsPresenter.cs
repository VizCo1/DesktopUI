using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingsPresenter : MonoBehaviour
{
    private SettingsDataSO _settingsDataSO;

    private void Start()
    {
        Initialize();


        // Event from View
        SettingsEvents.ResolutionDropdownChanged += SettingsEvents_ResolutionDropdownChanged;
    }

    private void OnDestroy()
    {
        // Event from View
        SettingsEvents.ResolutionDropdownChanged -= SettingsEvents_ResolutionDropdownChanged;
    }

    private void Initialize()
    {
        _settingsDataSO = Resources.Load<SettingsDataSO>("SettingsDataSO");
        SettingsEvents.ResolutionSet?.Invoke(_settingsDataSO.ResolutionsList, _settingsDataSO.GetResolutionIndex());
    }

    private void SettingsEvents_ResolutionDropdownChanged(int index)
    {
        SettingsEvents.ResolutionChanged?.Invoke(index);
        StartCoroutine(ChangeResolution(index));
    }


    private IEnumerator ChangeResolution(int index)
    {
        Resolution newResolution = _settingsDataSO.FilteredResolutions[index];

        if (newResolution.width != Screen.currentResolution.width && newResolution.height != Screen.currentResolution.height)
        {
            Screen.SetResolution(newResolution.width, newResolution.height,
                    FullScreenMode.FullScreenWindow, newResolution.refreshRateRatio);

            yield return new WaitForSeconds(0.2f);

            SettingsEvents.OnResolutionChanged?.Invoke();
        }

        yield return null;
    }
}
