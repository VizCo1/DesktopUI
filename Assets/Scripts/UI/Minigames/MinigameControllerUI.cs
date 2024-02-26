using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MinigameControllerUI : MonoBehaviour
{
    [SerializeField] private MinigameMenuUI _minigameMenuUI;
    [SerializeField] private MinigameHowToPlayUI _howToPlayUI;
    [SerializeField] private MinigameScoreboardUI _scoreboardUI;

    private void Start()
    {
        _minigameMenuUI.CanvasGroup.alpha = 1;
        _minigameMenuUI.gameObject.SetActive(true);
    }

    public void DoTransition(MinigameUI from, MinigameScreen to)
    {
        MinigameUI next = SelectScreen(to);

        float duration = 0.25f;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(DOVirtual.Float(from.CanvasGroup.alpha, 0, duration, (float value) => from.CanvasGroup.alpha = value));
        mySequence.AppendCallback(() => from.gameObject.SetActive(false));

        mySequence.AppendCallback(() => next.gameObject.SetActive(true));
        mySequence.Append(DOVirtual.Float(next.CanvasGroup.alpha, 1, duration, (float value) => next.CanvasGroup.alpha = value));
    }

    private MinigameUI SelectScreen(MinigameScreen minigameScreen)
    {
        if (_minigameMenuUI.GetMinigameScreen() == minigameScreen)
        {
            return _minigameMenuUI;
        }
        else if (_howToPlayUI.GetMinigameScreen() == minigameScreen)
        {
            return _howToPlayUI;
        }
        else
        {
            return _scoreboardUI;
        }
    }
}
