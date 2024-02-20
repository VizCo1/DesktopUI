using System;
using System.Collections.Generic;

public static class SettingsEvents
{
    // View --> Presenter
    public static Action<int> ResolutionDropdownChanged;

    // Model -> Presenter
    //public static Action<List<string>, int> ModelResolutionChanged;

    // Presenter --> View
    public static Action<List<string>, int> ResolutionSet;

    // Presenter --> Model
    public static Action<int> ResolutionChanged;

    // ----------------------------

    // Presenter --> Computer
    public static Action OnResolutionChanged;
}
