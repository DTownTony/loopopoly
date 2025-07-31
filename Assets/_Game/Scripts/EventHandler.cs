using UnityEngine;

public class EventHandler : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private CombatHandler _combatHandler;
    [SerializeField] private Player _player;

    public void AddGold(int amount)
    {
        _player.Data.Gold.Value += amount;
    }
    
    public void AddItem(ItemData itemData)
    {
        var item = new Item(itemData);
        _player.Data.AddItem(itemData);
    }

    public void StatUpdate(StatType type, int amount)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                _player.Data.UpdateMaxHealth(amount);
                break;
            case StatType.CurrentHealth:
                _player.Data.CurrentHealth.Value += amount;
                break;
            case StatType.Damage:
                _player.Data.Damage.Value += amount;
                break;
            case StatType.Defense:
                _player.Data.Defense.Value += amount;
                break;
        }
    }

    public void SetupCombat(CombatEvent combatEvent)
    {
        _combatHandler.StartCombat(combatEvent);
    }

    public void SetupBossCombat()
    {
        _combatHandler.StartCombat(GameController.Instance.CurrentLoopLevelData.BossData);
    }
}

public enum StatType
{
    MaxHealth,
    CurrentHealth,
    Damage,
    Defense,
}