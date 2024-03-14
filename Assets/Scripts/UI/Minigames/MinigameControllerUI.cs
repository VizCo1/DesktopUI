using DG.Tweening;
using System;
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
        InitScreens();

        // Activate first screen
        DoTransition(_minigameMenuUI, MinigameScreen.Menu, 0);
    }

    private void InitScreens()
    {
        _minigameMenuUI.Init();
        _howToPlayUI.Init();
        _scoreboardUI.Init();
    }

    public void DoTransition(MinigameUI from, MinigameScreen to, float duration = 0.25f)
    {
        MinigameUI next = GetScreen(to);

        Sequence transitionSequence = DOTween.Sequence();

        // Fade out active screen
        transitionSequence.Append(DOVirtual.Float(from.CanvasGroup.alpha, 0, duration, (float value) => from.CanvasGroup.alpha = value));
        transitionSequence.AppendCallback(() => from.gameObject.SetActive(false));

        // Fade in next screen
        transitionSequence.AppendCallback(() => next.gameObject.SetActive(true));
        transitionSequence.Append(DOVirtual.Float(next.CanvasGroup.alpha, 1, duration, (float value) => next.CanvasGroup.alpha = value));
    }

    private MinigameUI GetScreen(MinigameScreen minigameScreen)
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
