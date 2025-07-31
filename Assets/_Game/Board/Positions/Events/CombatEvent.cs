using UnityEngine;

[CreateAssetMenu(fileName = "CombatEvent", menuName = "Data/BoardEvent/CombatEvent")]
public class CombatEvent : BoardEvent
{
    public EnemyData EnemyData;
    
    public override void Trigger()
    {
        GameController.Instance.EventHandler.SetupCombat(this);
    }
}
