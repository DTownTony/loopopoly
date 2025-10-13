using UnityEngine;

[CreateAssetMenu(fileName = "TreasureEvent", menuName = "Data/BoardEvent/TreasureEvent")]
public class TreasureEvent : BoardEvent
{
    [SerializeField] private string _description;
    [SerializeField] private Sprite _cardSprite;
    [SerializeField] private ItemDatabase _itemDatabase;
    
    public override void Trigger()
    {
        GameController.Instance.EventHandler.EventView.ShowSelectEvent(
            new SelectEventUIArgs(Name, null, _description, 
                new []
                {
                    new SelectEventChoice()
                    {
                        Icon = _cardSprite,
                        OnSelect = SelectTrap
                    },
                    new SelectEventChoice()
                    {
                        Icon = _cardSprite,
                        OnSelect = SelectItem
                    },
                    new SelectEventChoice()
                    {
                        Icon = _cardSprite,
                        OnSelect = SelectGold
                    }
                }));
    }

    private void SelectTrap()
    {
        var healthDamage = Mathf.RoundToInt(GameController.Instance.Player.Data.CurrentHealth.Value * .25f);
        GameController.Instance.Player.Data.CurrentHealth.Value -= healthDamage;
        var col2 = new Color32(232, 25, 34,255);
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"Trap!\n-{healthDamage} Health",col:col2);
        //_audioSource.PlayOneShot(_trapSound, .35f);
    }

    private void SelectItem()
    {
        var item = _itemDatabase.GetRandomItem();
        GameController.Instance.Player.Data.AddItem(item);
        var col1 = new Color32(52, 155, 242,255);
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"+{item.Name}", col: col1);
        //_audioSource.PlayOneShot(_itemSound, .4f);
    }

    private void SelectGold()
    {
        var goldAmount = Random.Range(10, 25) * 10;
        GameController.Instance.Player.Data.Gold.Value += goldAmount;
        var col = new Color32(255, 220, 0,255);
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"+{goldAmount} Gold!", col: col);
        //_audioSource.PlayOneShot(_goldSound, 1f);
    }
}