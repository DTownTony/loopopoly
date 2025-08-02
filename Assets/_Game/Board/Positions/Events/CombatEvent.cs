using UnityEngine;

[CreateAssetMenu(fileName = "CombatEvent", menuName = "Data/BoardEvent/CombatEvent")]
public class CombatEvent : BoardEvent
{
    public override void Trigger()
    {
        GameController.Instance.EventHandler.SetupCombat();
    }
}
