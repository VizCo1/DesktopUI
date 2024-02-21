using System;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsEvents
{
    // View --> Presenter
    public static Action<int> ResolutionDropdownChanged;
    public static Action<int> DisplayModeDropdownChanged;
    public static Action<int> FrameRateDropdownChanged;
    public static Action<int> VSyncToggleChanged;
    public static Action<float> VolumeUISliderChanged;

    // Model -> Presenter
    public static Action<List<string>, int, Resolution> ModelResolutionChanged;
    public static Action<List<string>, int, FullScreenMode> ModelDisplayModeChanged;
    public static Action<List<int>, int, int> ModelFrameRateChanged;
    public static Action<bool> ModelVSyncChanged;
    public static Action<float> ModelVolumeUIChanged;

    // Presenter --> View
    public static Action<List<string>, int> ResolutionSet;
    public static Action<List<string>, int> DisplayModeSet;
    public static Action<List<int>, int> FrameRateSet;
    public static Action<bool> VSyncSet;
    public static Action<float> VolumeUISet;

    // Presenter --> Model
    public static Action<int> ResolutionChanged;
    public static Action<int> DisplayModeChanged;
    public static Action<int> FrameRateChanged;
    public static Action<bool> VSyncChanged;
    public static Action<float> VolumeUIChanged;

    // ----------------------------

    // Presenter --> Computer
    public static Action OnResolutionChanged;
}