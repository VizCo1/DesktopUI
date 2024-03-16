using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComputerController : MonoBehaviour
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

    // Minigames go first!
    private enum ApplicationWindow
    {
        Minigame1,
        Minigame2,
        Minigame3,
        Minigame4,
        Minigame5,
        Minigame6,
        Minigame7,
        Store,
        Settings,
    }

    [Header("Computer parts")]

    [SerializeField] private BarUI _barUI;
    [SerializeField] private DesktopUI _desktopUI;
    [SerializeField] private WindowsUI _windowsUI;

    [Header("Minigames")]

    [SerializeField] private ApplicationsIconInfoSO _applicationsIconInfoSO;
    [SerializeField] private RenderTexture[] _minigamesRenderTextures;

    private Computerstate _state = Computerstate.IsDesktop;

    private Dictionary<DesktopIcon, ApplicationInformation> _desktopIconAppInfoDictionary = new Dictionary<DesktopIcon, ApplicationInformation>();
    private Dictionary<BarIcon, ApplicationInformation> _barIconAppInfoDictionary = new Dictionary<BarIcon, ApplicationInformation>();

    private Canvas _mainCanvas;
    
    private ApplicationIcons _currentApplicationIcons;
    private int _lastMinigameIndex;

    private int _money;
    public static ComputerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            return;
        } 

        Instance = this;
        _mainCanvas = GetComponent<Canvas>();
        _currentApplicationIcons = new ApplicationIcons();

        // Money for the store
        _money = 9999;

        _lastMinigameIndex = _minigamesRenderTextures.Length - 1;
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

        // Start with 3 minigames
        foreach (ApplicationWindow applications in Enum.GetValues(typeof(ApplicationWindow)))
        {
            if (i >= availableApps)
            {
                break;
            }

            _desktopUI.AddIcon((int) applications, _applicationsIconInfoSO.applications[(int) applications]);
            i++;
        }

        // Store application
        _desktopUI.AddIcon((int) ApplicationWindow.Store, _applicationsIconInfoSO.applications[(int)ApplicationWindow.Store]);
        // Settings application
        _desktopUI.AddIcon((int) ApplicationWindow.Settings, _applicationsIconInfoSO.applications[(int)ApplicationWindow.Settings]);
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
            bool isMinigame = desktopIcon.ApplicationID <= _lastMinigameIndex;

            Window window;

            if (isMinigame)
            {
                window = _windowsUI.CreateMinigameWindow(_applicationsIconInfoSO.applications[desktopIcon.ApplicationID].name);
            }
            else
            {
                int index = desktopIcon.ApplicationID - _minigamesRenderTextures.Length; // index = ID - number of minigames
                window = _windowsUI.GetApplicationWindow(index);
            }

            // Current app icons
            _currentApplicationIcons.DesktopIcon = desktopIcon;
            _currentApplicationIcons.BarIcon = _barUI.AddIcon(desktopIcon.ApplicationID, 
                _applicationsIconInfoSO.applications[desktopIcon.ApplicationID]).GetComponent<BarIcon>();

            // Add to dictionaries
            ApplicationInformation appInfo = new ApplicationInformation(desktopIcon, _currentApplicationIcons.BarIcon, window);
            _desktopIconAppInfoDictionary.Add(_currentApplicationIcons.DesktopIcon, appInfo);
            _barIconAppInfoDictionary.Add(_currentApplicationIcons.BarIcon, appInfo);

            OpenWindowWithDesktopIcon(desktopIcon, isMinigame);
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

    private void OpenWindowWithDesktopIcon(DesktopIcon icon, bool isMinigame)
    {
        if (_desktopIconAppInfoDictionary.TryGetValue(icon, out ApplicationInformation appInfo))
        {
            SetIsWindowState();

            if (isMinigame)
            {
                MinigameWindow minigameWindow = appInfo.Window.GetComponent<MinigameWindow>();
                if (!minigameWindow.IsRenderTexturePrepared())
                {
                    ApplicationWindow appID = (ApplicationWindow) icon.ApplicationID;
                    minigameWindow.SetApplicationRenderTexture(GetMinigameRenderTexture(appID));
                    SceneManager.LoadScene(appID.ToString(), LoadSceneMode.Additive);
                }
                
                // Save next rawImage rectTransform
                MinigameInputHandlingHelper.SetNextRawImageRectTransform(minigameWindow.GetRawImageRectTransform());
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

            // Bar icon feedbacks are played when it is clicked !!
        }
        else
        {
            Debug.LogError("Error when trying to open a window, not found in dictionary");
        }
    }

    #endregion

    #region Close Window

    public void CloseWindowInstantEffects()
    {
        // Deactivate selected visuals of the previous bar icon
        _currentApplicationIcons.BarIcon.SetSelectedVisuals(false);

        _currentApplicationIcons.BarIcon.PlayAppearedFeedbacksAndDisable(MMFeedbacks.Directions.BottomToTop);

        SetIsDesktopState();
    }

    public void CloseWindowDelayedEffects(Window window)
    {
        window.ToDefault();

        bool isMinigame = _currentApplicationIcons.DesktopIcon.ApplicationID <= _lastMinigameIndex;

        if (isMinigame)
        {
            ApplicationWindow sceneToUnload = (ApplicationWindow) _currentApplicationIcons.DesktopIcon.ApplicationID;
            SceneManager.UnloadSceneAsync(sceneToUnload.ToString());
            _windowsUI.CloseMinigameWindow(window);
        }
        else
        {
            //_windowsUI.CloseApplicationWindow();
        }

        _barUI.FixPositionsAndRemoveIcon(_currentApplicationIcons.BarIcon);

        // Remove from dictionaries
        _desktopIconAppInfoDictionary.Remove(_currentApplicationIcons.DesktopIcon);
        _barIconAppInfoDictionary.Remove(_currentApplicationIcons.BarIcon);
    }

    #endregion

    #region Minimize window

    public void MinimizeWindowEffects()
    {
        // Deactivate selected visuals of the previous bar icon
        _currentApplicationIcons.BarIcon.SetSelectedVisuals(false);
    }

    #endregion

    private RenderTexture GetMinigameRenderTexture(ApplicationWindow application)
    {
        return _minigamesRenderTextures[(int)application];
    }

    public void AddMinigame(int appID)
    {
        _desktopUI.AddIcon(appID, _applicationsIconInfoSO.applications[appID]);
    }

    public Canvas GetMainCanvas() => _mainCanvas;

    public bool IsDesktopState() => _state == Computerstate.IsDesktop;
    public bool IsWindowState() => _state == Computerstate.IsWindow;
    public void SetIsDesktopState() => _state = Computerstate.IsDesktop;
    public void SetIsWindowState() => _state = Computerstate.IsWindow;

    public int Money { get => _money; set => _money = value; }
}
