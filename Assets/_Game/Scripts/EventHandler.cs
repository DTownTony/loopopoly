using UnityEngine;

public class EventHandler : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private CombatHandler _combatHandler;
    [SerializeField] private Player _player;
    [SerializeField] private EventView _eventView;

    public void AddGold(int amount)
    {
        _player.Data.Gold.Value += amount;
    }
    
    public void AddItem(ItemData itemData)
    {
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
    
    public void SetupBossCombat()
    {
        _combatHandler.StartCombat(GameController.Instance.LevelData.BossData);
    }
    
    public void SetupCombat()
    {
        _combatHandler.StartCombat(GameController.Instance.LevelData.EnemyData);
    }

    #region UI Views

    public void ShowShop()
    {
        _eventView.ShowShop();
    }

    public void ShowGrimReaper(GrimReaperUIArgs args)
    {
        _eventView.ShowGrimReaper(args);
    }
    
    public void ShowKingsTaxes(KingsTaxesUIArgs args)
    {
        _eventView.ShowKingsTaxes(args);
    }
    
    public void ShowTreasure()
    {
        _eventView.ShowTreasure();
    }

    public void ShowBasicEvent(EventUIArgs args)
    {
        _eventView.ShowBasicEvent(args);
    }

    #endregion
}

public enum StatType
{
    MaxHealth,
    CurrentHealth,
    CurrentHealthPercent,
    Damage,
    Defense,
    Experience
}