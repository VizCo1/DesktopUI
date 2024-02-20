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

    // Model -> Presenter
    //public static Action<int> ModelVSyncChanged;

    // Presenter --> View
    public static Action<List<string>, int> ResolutionSet;
    public static Action<List<string>, int> DisplayModeSet;
    public static Action<List<int>, int> FrameRateSet;
    public static Action<bool> VSyncSet;

    // Presenter --> Model
    //public static Action<int> ResolutionChanged;
    public static Action<int> DisplayModeChanged;
    public static Action<bool> VSyncChanged;

    // ----------------------------

    // Presenter --> Computer
    public static Action OnResolutionChanged;
}
