using UnityEngine;

public class MinigameMenuUI : MinigameUI
{
    [Header("Minigame Menu UI")]
    [SerializeField] private MinigameButton _playButton;
    [SerializeField] private MinigameButton _howToPlayButton;
    [SerializeField] private MinigameButton _scoreboardButton;

    private void Start()
    {
        _playButton.OnClicked += PlayButton_OnClicked;

        _howToPlayButton.OnClicked += HowToPlayButton_OnClicked;

        _scoreboardButton.OnClicked += ScoreboardButton_OnClicked;
    }

    private void PlayButton_OnClicked()
    {
        Debug.Log("Play button");
    }

    private void HowToPlayButton_OnClicked()
    {
        _minigameControllerUI.DoTransition(this, MinigameScreen.HowToPlay);
    }

    private void ScoreboardButton_OnClicked()
    {
        _minigameControllerUI.DoTransition(this, MinigameScreen.Scoreboard);
    }

    private void OnDestroy()
    {
        _playButton.OnClicked -= PlayButton_OnClicked;
        _howToPlayButton.OnClicked -= PlayButton_OnClicked;
        _scoreboardButton.OnClicked -= PlayButton_OnClicked;
    }
}
