using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public EventView EventView;
    
    [SerializeField] private GameController _gameController;
    [SerializeField] private CombatHandler _combatHandler;
    [SerializeField] private Player _player;

    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _goldSound;
    [SerializeField] private AudioClip _bonusSound;

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