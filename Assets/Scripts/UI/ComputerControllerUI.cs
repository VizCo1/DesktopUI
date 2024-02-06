using MoreMountains.Feedbacks;
using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerControllerUI : MonoBehaviour
{
    private class ApplicationIcons
    {
        public DesktopIcon DesktopIcon { get; set; }
        public BarIcon BarIcon { get; set; }
    }

    private class ApplicationInformation
    {
        public DesktopIcon DesktopIcon { get; private set; }
        public BarIcon BarIcon { get; private set; }
        public Window Window { get; private set; }

        public ApplicationInformation(DesktopIcon desktopIcon, BarIcon barIcon, Window window)
        {
            DesktopIcon = desktopIcon;
            BarIcon = barIcon;
            Window = window;
        }
    }

    private enum Computerstate
    {
        IsDesktop,
        IsWindow,
    }

    public enum MinigameScenes
    {
        Minigame1,
        Minigame2,
        Minigame3,
    }

    private Computerstate _state = Computerstate.IsDesktop;

    private Dictionary<DesktopIcon, ApplicationInformation> _desktopIconAppInfoDictionary = new Dictionary<DesktopIcon, ApplicationInformation>();
    private Dictionary<BarIcon, ApplicationInformation> _barIconAppInfoDictionary = new Dictionary<BarIcon, ApplicationInformation>();

    private Canvas _mainCanvas;

    [SerializeField] private BarUI _barUI;
    [SerializeField] private DesktopUI _desktopUI;
    [SerializeField] private WindowsUI _windowsUI;

    [SerializeField] private RenderTexture[] _minigamesRenderTextures;
    
    private ApplicationIcons _currentApplicationIcons;

    public static ComputerControllerUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        } 

        Instance = this;
        _mainCanvas = GetComponent<Canvas>();
        _currentApplicationIcons = new ApplicationIcons();
    }

    private void Start()
    {
        _barUI.InitializeSpace();
        _desktopUI.InitializeSpace();

        InitializeAvailableApplications();
    }

    private void InitializeAvailableApplications()
    {
        int availableApps = 3;
        int i = 0;
        foreach (MinigameScenes minigameScene in Enum.GetValues(typeof(MinigameScenes)))
        {
            if (i >= availableApps)
            {
                break;
            }

            _desktopUI.AddIcon((int) minigameScene);
            i++;
        }
    }

    #region Handle Icon Clicked

    public void HandleDesktopIconClicked(DesktopIcon desktopIcon)
    {
        // Desktop icons can open a window when in desktop
        if (!IsDesktopState())
        {
            return;
        }

        // Deactivate selected visuals of the previous bar icon
        if (_currentApplicationIcons.BarIcon != null)
        {
            _currentApplicationIcons.BarIcon.SetSelectedVisuals(false);
        }

        // Icon already has a window
        if (_desktopIconAppInfoDictionary.ContainsKey(desktopIcon))
        {
            // Since it has a window, the value is in the dictionary
            _desktopIconAppInfoDictionary.TryGetValue(desktopIcon, out ApplicationInformation appInfo); 

            // Current app icons
            _currentApplicationIcons.BarIcon = appInfo.BarIcon;
            _currentApplicationIcons.DesktopIcon = desktopIcon;

            OpenWindowWithDesktopIcon(desktopIcon, false);
        }
        // Create a new window
        else
        {
            Window window = _windowsUI.CreateWindow();

            // Current app icons
            _currentApplicationIcons.DesktopIcon = desktopIcon;
            _currentApplicationIcons.BarIcon = _barUI.AddIcon(desktopIcon.MinigameID).GetComponent<BarIcon>();

            // Add to dictionaries
            ApplicationInformation appInfo = new ApplicationInformation(desktopIcon, _currentApplicationIcons.BarIcon, window);
            _desktopIconAppInfoDictionary.Add(_currentApplicationIcons.DesktopIcon, appInfo);
            _barIconAppInfoDictionary.Add(_currentApplicationIcons.BarIcon, appInfo);

            OpenWindowWithDesktopIcon(desktopIcon, true);
        }

        // Activate the selected visuals of the current bar icon
        _currentApplicationIcons.BarIcon.SetSelectedVisuals(true);
    }

    public void HandleBarIconClicked(BarIcon barIcon)
    {

        // Deactivate selected visuals of the previous bar icon
        _currentApplicationIcons.BarIcon.SetSelectedVisuals(false);

        if (IsWindowState() && _barIconAppInfoDictionary.TryGetValue(_currentApplicationIcons.BarIcon, out ApplicationInformation appInfo))
        {
            appInfo.Window.Minimize();
          
            // Same icons, just minimize the window
            if (barIcon == _currentApplicationIcons.BarIcon)
            {
                return;
            }
            // Update current app icons
            else
            {
                _currentApplicationIcons.DesktopIcon = appInfo.DesktopIcon;
                _currentApplicationIcons.BarIcon = barIcon;
            }
        }

        // Icon has a window
        if (_barIconAppInfoDictionary.ContainsKey(barIcon))
        {
            // Since it has a window, the value is in the dictionary
            _barIconAppInfoDictionary.TryGetValue(barIcon, out appInfo);

            // Current app icons
            _currentApplicationIcons.DesktopIcon = appInfo.DesktopIcon;
            _currentApplicationIcons.BarIcon = barIcon;

            OpenWindowWithBarIcon(barIcon);
        }
        else
        {
            Debug.LogError("This bar icon should not be activated, it does not have a window");
        }

        // Activate the selected visuals of the current bar icon
        _currentApplicationIcons.BarIcon.SetSelectedVisuals(true);
    }

    #endregion

    #region Open Window

    private void OpenWindowWithDesktopIcon(DesktopIcon icon, bool isFirstTime)
    {
        if (_desktopIconAppInfoDictionary.TryGetValue(icon, out ApplicationInformation appInfo))
        {
            SetIsWindowState();

            if (isFirstTime)
            {
                MinigameScenes sceneToLoad = (MinigameScenes) icon.MinigameID;
                appInfo.Window.SetMinigameRenderTexture(GetMinigameRenderTexture(sceneToLoad));
                SceneManager.LoadScene(sceneToLoad.ToString(), LoadSceneMode.Additive);
            }

            appInfo.Window.gameObject.SetActive(true);
            appInfo.Window.Open();
            appInfo.BarIcon.PlayAppearedFeedbacks(MMFeedbacks.Directions.TopToBottom);
        }
        else
        {
            Debug.LogError("Error when trying to open a window, not found in dictionary");
        }
    }

    private void OpenWindowWithBarIcon(BarIcon icon)
    {
        if (_barIconAppInfoDictionary.TryGetValue(icon, out ApplicationInformation appInfo))
        {
            appInfo.Window.gameObject.SetActive(true);
            SetIsWindowState();
            appInfo.Window.Open();
            // Bar icon feedbacks are played when it is clicked
        }
        else
        {
            Debug.LogError("Error when trying to open a window, not found in dictionary");
        }
    }

    #endregion

    #region Close Window

    public void CloseWindowEffects()
    {
        // Deactivate selected visuals of the previous bar icon
        _currentApplicationIcons.BarIcon.SetSelectedVisuals(false);

        _currentApplicationIcons.BarIcon.PlayAppearedFeedbacksAndDisable(MMFeedbacks.Directions.BottomToTop);
    }

    public void CloseWindow(Window window)
    {
        window.ToDefault();

        MinigameScenes sceneToUnload = (MinigameScenes) _currentApplicationIcons.DesktopIcon.MinigameID;
        SceneManager.UnloadSceneAsync(sceneToUnload.ToString());

        _windowsUI.CloseWindow(window);

        _barUI.FixPositionsAndRemoveIcon(_currentApplicationIcons.BarIcon);

        // Remove from dictionaries
        _desktopIconAppInfoDictionary.Remove(_currentApplicationIcons.DesktopIcon);
        _barIconAppInfoDictionary.Remove(_currentApplicationIcons.BarIcon);

        SetIsDesktopState();
    }

    #endregion

    #region Minimize window

    public void MinimizeWindowEffects()
    {
        // Deactivate selected visuals of the previous bar icon
        _currentApplicationIcons.BarIcon.SetSelectedVisuals(false);
    }

    #endregion

    private RenderTexture GetMinigameRenderTexture(MinigameScenes minigameScenes)
    {
        return _minigamesRenderTextures[(int)minigameScenes];
    }

    public Canvas GetMainCanvas() => _mainCanvas;

    public bool IsDesktopState() => _state == Computerstate.IsDesktop;
    public bool IsWindowState() => _state == Computerstate.IsWindow;
    public void SetIsDesktopState() => _state = Computerstate.IsDesktop;
    public void SetIsWindowState() => _state = Computerstate.IsWindow;
}
