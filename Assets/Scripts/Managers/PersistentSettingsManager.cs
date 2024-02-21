using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentSettingsManager
{
    private static string RESOLUTION_WIDTH = "ResolutionWidth";
    private static string RESOLUTION_HEIGHT = "ResolutionHeight";

    private static string DISPLAY_MODE = "DisplayMode";

    private static string SYSTEM_VOLUME = "SystemVolume";

    private static string FRAME_RATE = "FrameRate";

    private static string VSYNC = "VSync";


    public static Resolution LoadResolution()
    {
        int width = PlayerPrefs.GetInt(RESOLUTION_WIDTH, -1);
        int height = PlayerPrefs.GetInt(RESOLUTION_HEIGHT, -1);

        Resolution res = new Resolution();
        res.width = width;
        res.height = height;
        res.refreshRateRatio = Screen.currentResolution.refreshRateRatio;

        if (width < 0 || height < 0)
        {
            return Screen.currentResolution;
        }

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
