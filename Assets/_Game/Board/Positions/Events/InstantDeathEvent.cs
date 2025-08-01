using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Data/BoardEvent/InstantDeath", fileName = "InstantDeath")]
public class InstantDeathEvent : BoardEvent
{
    [SerializeField] private ItemData _itemNeededToStopEvent;
    
    public override void Trigger()
    {
        var deathChance = .25f;
        for (var i = 0; i < GameController.Instance.MaxLoops; i++)
            deathChance += Random.Range(.05f, .1f);
        
        GameController.Instance.EventHandler.ShowGrimReaper(
            new GrimReaperUIArgs(_itemNeededToStopEvent, deathChance));
    }
}