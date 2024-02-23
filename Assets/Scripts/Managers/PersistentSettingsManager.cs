using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentSettingsManager
{
    private const string RESOLUTION_WIDTH = "ResolutionWidth";
    private const string RESOLUTION_HEIGHT = "ResolutionHeight";

    private const string DISPLAY_MODE = "DisplayMode";

    private const string SYSTEM_VOLUME = "SystemVolume";

    private const string FRAME_RATE = "FrameRate";

    private const string VSYNC = "VSync";

    public static Resolution LoadResolution()
    {
        int width = PlayerPrefs.GetInt(RESOLUTION_WIDTH, 1920);
        int height = PlayerPrefs.GetInt(RESOLUTION_HEIGHT, 1080);

        Resolution res = new Resolution()
        {
            width = width,
            height = height,
            refreshRateRatio = Screen.currentResolution.refreshRateRatio
        };

        return res;
    }

    public static FullScreenMode LoadDisplayMode()
    {
        return PlayerPrefs.GetInt(DISPLAY_MODE, 0) == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public static float LoadSystemVolume()
    {
        return PlayerPrefs.GetFloat(SYSTEM_VOLUME, 1);
    }

    public static int LoadFrameRate()
    {
        return PlayerPrefs.GetInt(FRAME_RATE, 60);
    }

    public static bool LoadVSync()
    {
        return PlayerPrefs.GetInt(VSYNC, 0) == 1;
    }

    public static void SaveResolution(Resolution res)
    {
        int width = res.width;
        int height = res.height;

        // Resolution
        PlayerPrefs.SetInt(RESOLUTION_WIDTH, width);
        PlayerPrefs.SetInt(RESOLUTION_HEIGHT, height);
    }

    public static void SaveDisplayMode(FullScreenMode displayMode)
    {
        // Display mode
        PlayerPrefs.SetInt(DISPLAY_MODE, displayMode == FullScreenMode.FullScreenWindow ? 0 : 1);
    }

    public static void SaveSystemVolume(float systemVolume)
    {
        // System volume (UI)
        PlayerPrefs.SetFloat(SYSTEM_VOLUME, systemVolume);       
    }

    public static void SaveFrameRate(int frameRate)
    {
        // Frame rate
        PlayerPrefs.SetInt(FRAME_RATE, frameRate);
    }

    public static void SaveVSync(bool isVSync)
    {
        // VSync
        PlayerPrefs.SetInt(VSYNC, isVSync ? 1 : 0);
    }
}
