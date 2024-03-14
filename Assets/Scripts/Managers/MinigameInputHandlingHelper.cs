using UnityEngine;

public static class MinigameInputHandlingHelper
{
    private static RectTransform _nextRawImageRectTransform;

    public static void SetNextRawImageRectTransform(RectTransform rawImageRectTransform)
    {
        _nextRawImageRectTransform = rawImageRectTransform;
    }

    public static RectTransform GetNextRawImageRectTransform()
    {
        return _nextRawImageRectTransform;
    }
}
