using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Data/BoardEvent/InstantDeath", fileName = "InstantDeath")]
public class InstantDeathEvent : BoardEvent
{
    [SerializeField] private ItemData _itemNeededToStopEvent;
    
    public override void Trigger()
    {
        var deathChance = .2f;
        for (var i = 0; i < GameController.Instance.MaxLoops; i++)
            deathChance += Random.Range(.01f, .05f);
        
        GameController.Instance.EventHandler.ShowGrimReaper(
            new GrimReaperUIArgs(_itemNeededToStopEvent, deathChance));
    }
}