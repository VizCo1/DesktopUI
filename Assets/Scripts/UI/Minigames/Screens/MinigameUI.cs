using UnityEngine;

public class MinigameUI : MonoBehaviour
{
    [Header("Minigame UI")]
    [SerializeField] protected MinigameControllerUI _minigameControllerUI;
    [SerializeField] private MinigameScreen _minigameScreen;

    public CanvasGroup CanvasGroup { get; private set; }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        CanvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public MinigameScreen GetMinigameScreen() => _minigameScreen;
}
