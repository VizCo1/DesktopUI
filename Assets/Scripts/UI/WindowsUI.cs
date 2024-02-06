using System;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WindowsUI : MonoBehaviour
{
    private Pool _windowPool;

    private void Awake()
    {
        _windowPool = GetComponent<Pool>();
    }

    public Window CreateWindow()
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

    public void CloseWindow(Window window)
    {
        _windowPool.Enqueue(window.gameObject);
    }
}
