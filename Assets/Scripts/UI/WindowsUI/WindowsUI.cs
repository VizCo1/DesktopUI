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
        MinigameWindow minigameWindow;

        if (_windowPool.TryDequeue(out GameObject windowGO))
        {
            minigameWindow = windowGO.GetComponent<MinigameWindow>();
        }
        else
        {
            minigameWindow = _windowPool.CreateGameObject().GetComponent<MinigameWindow>();        
        }

        // Since minigames windows are generic the title text needs to be updated
        minigameWindow.SetTitle(title);

        return minigameWindow;
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
