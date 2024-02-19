using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsDataSO", menuName = "ScriptableObjects/SettingsDataSO")]
public class SettingsDataSO : ScriptableObject
{

    private List<string> _resolutionsList;
    private List<Resolution> _filteredResolutions;

    // Properties
    public List<string> ResolutionsList { get => _resolutionsList; set => _resolutionsList = value; }
    public List<Resolution> FilteredResolutions { get => _filteredResolutions; set => _filteredResolutions = value; }

    private void OnEnable()
    {
        InitializeFilteredResolutions();
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
}
