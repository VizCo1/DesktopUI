using System;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WindowsUI : MonoBehaviour
{
    private Pool _windowPool;

    [SerializeField] private Window[] _noMinigameWindows;

    private void Awake()
    {
        _windowPool = GetComponent<Pool>();
    }

    public Window CreateMinigameWindow()
    {
        Window window;

        if (_windowPool.TryDequeue(out GameObject windowGO))
        {
            window = windowGO.GetComponent<Window>();
        }
        else
        {
            window = _windowPool.CreateGameObject().GetComponent<Window>();
        }

        return window;
    }

    public Window CreateApplicationWindow(int index)
    {
        return _noMinigameWindows[index];
    }

    public void CloseMinigameWindow(Window window)
    {
        _windowPool.Enqueue(window.gameObject);
    }

    //public void CloseApplicationWindow()
    //{

    //}
}
