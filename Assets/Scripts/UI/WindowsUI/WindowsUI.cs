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

    public Window CreateMinigameWindow(string title)
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

        // Since minigames windows are generic the title text needs to be updated
        window.SetTitle(title);

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
