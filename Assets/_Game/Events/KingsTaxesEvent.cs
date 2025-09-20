using UnityEngine;

[CreateAssetMenu(fileName = "KingsTaxesEvent", menuName = "Data/BoardEvent/KingsTaxesEvent")]
public class KingsTaxesEvent : BoardEvent
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private ItemData _stopEventItem;
    
    private int _ownedGoldAmount;
    
    public override void Trigger()
    {
        _ownedGoldAmount = Mathf.RoundToInt(GameController.Instance.Player.Data.Gold.Value * .5f);
        
        GameController.Instance.EventHandler.ShowChoiceEvent(new ChoiceEventUIArgs(
            Name, _sprite, $"The king has demanded taxes be paid. You owe {_ownedGoldAmount} gold!", 
            UseItem, PayTax,
            $"Use {_stopEventItem.Name}!", 
            $"Pay {_ownedGoldAmount} gold!",
            GameController.Instance.Player.Data.HasItem(_stopEventItem.Key)));
    }
    
    private void UseItem()
    {
        GameController.Instance.Player.Data.RemoveItem(_stopEventItem.Key);
    }

    private void PayTax()
    {
        GameController.Instance.Player.Data.Gold.Value -= _ownedGoldAmount;
        //_audioSource.PlayOneShot(_goldSound, 1f);
    }
}