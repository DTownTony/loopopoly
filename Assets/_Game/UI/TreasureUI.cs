using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TreasureUI : MonoBehaviour
{
    [SerializeField] private EventView _eventView;
    [SerializeField] private Button[] _treasureButtons;
    [SerializeField] private ItemDatabase _itemDatabase;
    
    public void Show()
    {
        gameObject.SetActive(true);

        var availableTreasureTypes = new List<TreasureType>()
        {
            TreasureType.Gold,
            TreasureType.Item,
            TreasureType.Trap
        };
        
        foreach (var button in _treasureButtons)
        {
            button.enabled = true;
            button.onClick.RemoveAllListeners();
            var randomType = availableTreasureTypes[Random.Range(0, availableTreasureTypes.Count)];
            availableTreasureTypes.Remove(randomType);
            button.onClick.AddListener(()=> SelectTreasureType(randomType));
        }
    }

    private void SelectTreasureType(TreasureType treasureType)
    {
        foreach (var button in _treasureButtons)
            button.enabled = false;
        
        switch (treasureType)
        {
            case TreasureType.Gold:
                var goldAmount = Random.Range(10, 25) * 10;
                GameController.Instance.Player.Data.Gold.Value += goldAmount;
                GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"+{goldAmount} Gold!");
                break;
            case TreasureType.Item:
                var item = _itemDatabase.GetRandomItem();
                GameController.Instance.Player.Data.AddItem(item);
                GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"+{item.Name}");
                break;
            case TreasureType.Trap:
                var healthDamage = Mathf.RoundToInt(GameController.Instance.Player.Data.CurrentHealth.Value * .25f);
                GameController.Instance.Player.Data.CurrentHealth.Value -= healthDamage;
                GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"Trap! -{healthDamage} Health;");
                break;
        }
        
        Hide();
    }

    private void Hide()
    {
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
        gameObject.SetActive(false);
        _eventView.Hide();
    }

    private enum TreasureType
    {
        Gold,
        Item,
        Trap
    }
}
