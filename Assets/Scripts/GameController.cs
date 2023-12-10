using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] private Canvas _mainCanvas;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Canvas GetMainCanvas() => _mainCanvas;
}
