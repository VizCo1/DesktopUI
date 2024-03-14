using UnityEngine;

public class MinigameHowToPlayUI : MinigameUI
{
    [Header("How to play UI")]
    [SerializeField] private MinigameButton _backButton;

    public override void Init()
    {
        base.Init();

        _backButton.OnClicked += BackButton_OnClicked;
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
