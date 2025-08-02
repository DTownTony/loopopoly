using UnityEngine;

[CreateAssetMenu(fileName = "KingsTaxesEvent", menuName = "Data/BoardEvent/KingsTaxesEvent")]
public class KingsTaxesEvent : BoardEvent
{
    [SerializeField] private ItemData _stopEventItem;
    
    public override void Trigger()
    {
        var amount = Mathf.RoundToInt(GameController.Instance.Player.Data.Gold.Value * .25f);
        GameController.Instance.EventHandler.ShowKingsTaxes(new KingsTaxesUIArgs(_stopEventItem, amount));
    }
}