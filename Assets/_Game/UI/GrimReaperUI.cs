using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GrimReaperUI : MonoBehaviour
{
    [SerializeField] private EventView _eventView;
    [SerializeField] private Button _tryButton;
    [SerializeField] private Button _guardianAngelButton;
    [SerializeField] private TMP_Text _deathChanceText;
    
    private GrimReaperUIArgs _currentArgs;

    private void Awake()
    {
        _tryButton.onClick.AddListener(TryButtonPressed);
        _guardianAngelButton.onClick.AddListener(GuardianAngelButtonPressed);
    }

    public void Show(GrimReaperUIArgs args)
    {
        gameObject.SetActive(true);
        _currentArgs = args;
        _guardianAngelButton.interactable = GameController.Instance.Player.Data.HasItem(args.StopEventItem.Key);
        _deathChanceText.SetText("Try My Luck: " + (args.DeathChance * 100).ToString("F2") + "%");
    }

    private void TryButtonPressed()
    {
        if (Random.value >= _currentArgs.DeathChance)
        {
            Hide();
        }
        else
        {
            //todo: gameover screen
            GameController.Instance.ReloadGame();
        }
    }

    private void GuardianAngelButtonPressed()
    {
        GameController.Instance.Player.Data.RemoveItem(_currentArgs.StopEventItem.Key);
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        _eventView.Hide();
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }
}

public class GrimReaperUIArgs
{
    public ItemData StopEventItem { get; set; }
    public float DeathChance { get; set; }

    public GrimReaperUIArgs(ItemData stopEventItem, float deathChance)
    {
        StopEventItem = stopEventItem;
        DeathChance = deathChance;
    }
}