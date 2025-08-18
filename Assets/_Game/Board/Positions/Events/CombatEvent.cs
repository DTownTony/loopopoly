using UnityEngine;

[CreateAssetMenu(fileName = "CombatEvent", menuName = "Data/BoardEvent/CombatEvent")]
public class CombatEvent : BoardEvent
{
    [SerializeField] private CombatDifficulty _combatDifficulty;
    
    public override void Trigger()
    {
        GameController.Instance.EventHandler.SetupCombat(_combatDifficulty);
    }
}

public enum CombatDifficulty
{
    Easy,
    Medium,
    Hard
}