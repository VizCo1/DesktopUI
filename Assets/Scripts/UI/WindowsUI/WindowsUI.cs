using UnityEngine;

public class WindowsUI : MonoBehaviour
{
    private Pool _minigameWindowPool;

    [SerializeField] private Window[] _noMinigameWindows;

    private void Awake()
    {
        _minigameWindowPool = GetComponent<Pool>();
    }

    public Window CreateMinigameWindow(string title)
    {
        MinigameWindow minigameWindow;

        if (_minigameWindowPool.TryDequeue(out GameObject windowGO))
        {
            minigameWindow = windowGO.GetComponent<MinigameWindow>();
        }
        else
        {
            minigameWindow = _minigameWindowPool.CreateGameObject().GetComponent<MinigameWindow>();        
        }

        // Since minigames windows are generic the title text needs to be updated
        minigameWindow.SetTitle(title);

        return minigameWindow;
    }

    public Window GetApplicationWindow(int index)
    {
        return _noMinigameWindows[index];
    }

    public void CloseMinigameWindow(Window window)
    {
        _minigameWindowPool.Enqueue(window.gameObject);
    }

    //public void CloseApplicationWindow()
    //{

    //}
}
