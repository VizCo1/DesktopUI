using TMPro;
using UnityEngine;

public class MinigameScoreboardUI : MinigameUI
{
    private const int NAME_POSITION = 2;
    private const int POINTS_POSITION = 3;

    [Header("Scoreboard UI")]
    [SerializeField] private MinigameButton _backButton;
    [SerializeField] private Transform _scoreboard;
    [SerializeField] private ScoreboardDataSO _scoreboardDataSO;

    public override void Init()
    {
        base.Init();

        _backButton.OnClicked += BackButton_OnClicked;

        InitScoreboard();
    }

    private void InitScoreboard()
    {
        for (int i = 0; i < _scoreboard.childCount; i++)
        {
            Transform field = _scoreboard.GetChild(i);
            ScoreboardDataSO.ScoreboardField scoreboardField = _scoreboardDataSO.GetScoreboardFieldAt(i);
            field.GetChild(NAME_POSITION).GetComponent<TMP_Text>().SetText(scoreboardField.name);
            field.GetChild(POINTS_POSITION).GetComponent<TMP_Text>().SetText(scoreboardField.score.ToString());
        }
    }

    private void BackButton_OnClicked()
    {
        _minigameControllerUI.DoTransition(this, MinigameScreen.Menu);
    }

    private void OnDestroy()
    {
        _backButton.OnClicked -= BackButton_OnClicked;
    }
}
