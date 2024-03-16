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
        DisableSoldApplicationsButtons();
    }

    public override void Open()
    {
        base.Open();
        _moneyText.SetText(ComputerController.Instance.Money.ToString());
    }

    private void PrepareApplicationsButtons()
    {
        for (int i = 0; i < _appContainer.childCount; i++)
        {
            Button button = _appContainer.GetChild(i).GetComponent<Button>();

            int appID = i;
            button.onClick.AddListener(() =>
            {
                if (ComputerController.Instance.IsWindowState())
                {
                    button.interactable = false;
                    UnlockMinigame(appID);
                    SoundsManager.Instance.PlayUISound();
                }
            });
        }
    }

    private void DisableSoldApplicationsButtons()
    {
        for (int i = 0; i < _appContainer.childCount; i++)
        {
            if (_minigameInfoSO.minigames[i].sold)
            {
                _appContainer.GetChild(i).GetComponent<Button>().interactable = false;
            }
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
        if (ComputerController.Instance.Money >= _minigameInfoSO.minigames[appID].cost)
        {
            ComputerController.Instance.Money -= _minigameInfoSO.minigames[appID].cost;
            _moneyText.SetText(ComputerController.Instance.Money.ToString());
            ComputerController.Instance.AddMinigame(appID);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // Remove listeners from application buttons
        for (int i = 0; i < _appContainer.childCount; i++)
        {
            Button button = _appContainer.GetChild(i).GetComponent<Button>();

            button.onClick.RemoveAllListeners();
        }
    }
}
