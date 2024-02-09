using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreWindow : Window
{
    private const int ICON_INDEX = 2;
    private const int NAME_INDEX = 3;
    private const int DESCRIPTION_INDEX = 4;
    private const int MONEY_TEXT_INDEX = 5;

    [Header("Store")]

    [SerializeField] private Transform _appContainer;
    [SerializeField] private MinigamesStoreInfoSO _minigameInfoSO;
    [SerializeField] private TMP_Text _moneyText;

    protected override void Start()
    {
        base.Start();

        FillApplicationsInfo();
        PrepareApplicationsButtons();
    }

    public override void Open()
    {
        base.Open();
        _moneyText.SetText(ComputerControllerUI.Instance.Money.ToString());
    }

    private void PrepareApplicationsButtons()
    {
        for (int i = 0; i < _appContainer.childCount; i++)
        {
            Button button = _appContainer.GetChild(i).GetComponent<Button>();

            int appID = i;
            button.onClick.AddListener(() =>
            {
                if (ComputerControllerUI.Instance.IsWindowState())
                {
                    button.interactable = false;
                    UnlockMinigame(appID);
                }
            });
        }
    }

    private void FillApplicationsInfo()
    {
        for (int i = 0; i < _appContainer.childCount; i++)
        {
            Transform application = _appContainer.GetChild(i);

            Image icon = application.GetChild(ICON_INDEX).GetComponent<Image>();
            TMP_Text name = application.GetChild(NAME_INDEX).GetComponent<TMP_Text>();
            TMP_Text description = application.GetChild(DESCRIPTION_INDEX).GetComponent<TMP_Text>();
            TMP_Text money = application.GetChild(MONEY_TEXT_INDEX).GetComponent<TMP_Text>();

            icon.sprite = _minigameInfoSO.minigames[i].icon;
            name.SetText(_minigameInfoSO.minigames[i].name);
            description.SetText(_minigameInfoSO.minigames[i].description);
            money.SetText(_minigameInfoSO.minigames[i].cost.ToString());
        }
    }

    private void UnlockMinigame(int appID)
    {
        if (ComputerControllerUI.Instance.Money >= _minigameInfoSO.minigames[appID].cost)
        {
            ComputerControllerUI.Instance.Money -= _minigameInfoSO.minigames[appID].cost;
            _moneyText.SetText(ComputerControllerUI.Instance.Money.ToString());
            ComputerControllerUI.Instance.AddMinigame(appID);
        }
    }
}
