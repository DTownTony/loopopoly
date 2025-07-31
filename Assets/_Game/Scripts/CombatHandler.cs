using System.Collections;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    private Enemy _enemy;
    
    public void StartCombat(CombatEvent combatEvent)
    {
        StartCombat(combatEvent.EnemyData);
    }

    public void StartCombat(EnemyData enemyData)
    {
        GameController.Instance.ChangeCurrentState(GameState.Combat);
        _enemy = new Enemy()
        {
            Data = enemyData,
            CurrentHealth = enemyData.Health
        };
        
        var position = _player.transform.position;
        _enemy.Model = Instantiate(_enemy.Data.Prefab, position, Quaternion.identity);

        StartCoroutine(CombatSequence());
    }

    private IEnumerator CombatSequence()
    {
        _player.MoveForCombat();
        yield return new WaitForSeconds(.2f);

        var playerTurn = false;
        var combatActive = true;
        while (combatActive)
        {
            int damage;
            if (playerTurn)
            {
                damage = _player.Data.Damage.Value;
                _enemy.CurrentHealth -= damage;
                playerTurn = false;
            }
            else
            {
                damage = Random.Range(_enemy.Data.DamageMin, _enemy.Data.DamageMax);
                _player.Data.CurrentHealth.Value -= damage;
                playerTurn = true;
            }
            
            combatActive = _enemy.CurrentHealth > 0 && _player.Data.CurrentHealth.Value > 0;
            yield return new WaitForSeconds(.5f);
        }

        if (_player.Data.CurrentHealth.Value <= 0)
            CombatLose();
        else
            CombatWin();
        
        yield return new WaitForSeconds(.2f);
        
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }

    private void CombatWin()
    {
        _player.MoveOutCombat();
        _player.Data.Experience.Value += _enemy.Data.Experience;
      
        Destroy(_enemy.Model.gameObject);
        _enemy = null;
    }

    private void CombatLose()
    {
        GameController.Instance.ReloadScene();
    }

    private class Enemy
    {
        public EnemyData Data;
        public int CurrentHealth;
        public GameObject Model;
    }
}