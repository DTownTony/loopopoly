using UnityEngine;

public class EventHandler : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private CombatHandler _combatHandler;
    [SerializeField] private Player _player;
    [SerializeField] private EventView _eventView;

    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private  AudioClip _goldSound;
    [SerializeField] private  AudioClip _bonusSound;

    public void AddGold(int amount)
    {
        _player.Data.Gold.Value += amount;
        _audioSource.PlayOneShot(_goldSound, 1f);
    }
    
    public void AddItem(ItemData itemData)
    {
        _player.Data.AddItem(itemData);
        _audioSource.PlayOneShot(_bonusSound, .4f);
    }

    public void StatUpdate(StatType type, int amount)
    {
        switch (type)
        {
            case StatType.MaxHealth:
                _player.Data.MaxHealth.Value += amount;
                break;
            case StatType.CurrentHealth:
                _player.Data.CurrentHealth.Value += amount;
                break;
            case StatType.Damage:
                _player.Data.Damage.Value += amount;
                break;
            case StatType.Protection:
                _player.Data.Protection.Value += amount;
                break;
        }
        
        _audioSource.PlayOneShot(_bonusSound, .4f);
    }
    
    public void SetupBossCombat()
    {
        _combatHandler.StartCombat(GameController.Instance.LevelData.BossData);
    }
    
    public void SetupCombat(CombatDifficulty difficulty)
    {
        _combatHandler.StartCombat(GameController.Instance.LevelData.GetEnemyDifficulty(difficulty));
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
    Protection,
    Experience
}