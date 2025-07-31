using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoardEvent/InstantDeath", fileName = "InstantDeath")]
public class InstantDeathEvent : BoardEvent
{
    [SerializeField] private ItemData _itemNeededToStopEvent;
    
    public override void Trigger()
    {
        base.Trigger();
        
        var player = GameController.Instance.Player;
        if (player.Data.HasItem(_itemNeededToStopEvent.Key))
        {
            player.Data.RemoveItem(_itemNeededToStopEvent.Key);
            return;
        }

        GameController.Instance.ReloadScene();
        Debug.Log("Instant Death Event Triggered: Player has died.");
    }
}