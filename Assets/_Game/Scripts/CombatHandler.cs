using System.Collections;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ItemData _bombItem;
    
    private Enemy _enemy;
    
    public void StartCombat(EnemyData enemyData)
    {
        GameController.Instance.ChangeCurrentState(GameState.Combat);
        _enemy = new Enemy(enemyData, GameController.Instance.MaxLoops);
        
        var position = _player.transform.position;
        _enemy.Model = Instantiate(enemyData.Prefab, position, Quaternion.identity);

        StartCoroutine(CombatSequence());
    }

    private IEnumerator CombatSequence()
    {
        _player.MoveForCombat();
        yield return new WaitForSeconds(.2f);

        var playerTurn = false;
        var combatActive = true;
        
        //todo: bomb effect
        if (_player.Data.HasItem(_bombItem.Key))
        {
            Debug.Log("Use bomb!");
            combatActive = false;
            _player.Data.RemoveItem(_bombItem.Key);
        }
        
        while (combatActive)
        {
            int damage;
            if (playerTurn)
            {
                damage = _player.Data.Damage.Value;
                _enemy.CurrentHealth -= damage;
                playerTurn = false;
                GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"-{damage}", _enemy.Model.transform);
            }
            else
            {
                damage = Random.Range(_enemy.DamageMin, _enemy.DamageMax);
                damage = Mathf.Max(0, damage - _player.Data.Defense.Value);
                _player.Data.CurrentHealth.Value -= damage;
                playerTurn = true;
                GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"-{damage}", _player.Model);
            }
            
            combatActive = _enemy.CurrentHealth > 0 && _player.Data.CurrentHealth.Value > 0;
            yield return new WaitForSeconds(.75f);
        }
        
        //player won. Loss is handled with Player
        _player.MoveOutCombat();
        _player.Data.Experience.Value += _enemy.Experience;
        
        if(_enemy.IsBoss)
            GameController.Instance.ChangeLevelLoop();
      
        Destroy(_enemy.Model.gameObject);
        _enemy = null;
        
        yield return new WaitForSeconds(.2f);
        
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }
    
    private class Enemy
    {
        public int Experience => _enemyData.Experience;
        public bool IsBoss => _enemyData.IsBoss;
        
        public int CurrentHealth;
        public readonly int DamageMin;
        public readonly int DamageMax;
        public GameObject Model;

        private readonly EnemyData _enemyData;
        
        public Enemy(EnemyData enemyData, int loops)
        {
            _enemyData = enemyData;
            CurrentHealth = Mathf.RoundToInt(_enemyData.Health * Mathf.Pow(GameController.LOOP_COMBAT_VALUE, loops));
            DamageMin = Mathf.RoundToInt(_enemyData.DamageMin * Mathf.Pow(GameController.LOOP_COMBAT_VALUE, loops));
            DamageMax = Mathf.RoundToInt(_enemyData.DamageMax * Mathf.Pow(GameController.LOOP_COMBAT_VALUE, loops));
            Debug.Log("Base: " + _enemyData.DamageMin + " - " + _enemyData.DamageMax + " | " + DamageMin + " - " + DamageMax);
        }
    }
}