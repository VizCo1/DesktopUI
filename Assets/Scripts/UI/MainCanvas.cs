using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class MainCanvas : MonoBehaviour
{
    private Dictionary<DesktopIcon, Window> _iconWindowDictionary = new Dictionary<DesktopIcon, Window>();

    private DesktopUI _desktopUI;

    [Button]
    public void AddItemToDictionary(DesktopIcon icon)
    {
        Window window = new Window();
        //_desktopUI.AddIcon(icon);
        _iconWindowDictionary.Add(icon, window);
    }
}
