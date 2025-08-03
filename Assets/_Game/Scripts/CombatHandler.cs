using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ItemData _bombItem;
    [SerializeField] private ItemData _reviveItem;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private ParticleSystem _slashAttack;
    [SerializeField] private ParticleSystem _bombAttackPrefab;
    [SerializeField] private EnemyHealthBarUI _healthBarUI;
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _hitSounds;
    [SerializeField] private AudioClip _missSound;
    [SerializeField] private AudioClip _bossDefeatSound;
    
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
        
        _healthBarUI.Show();

        var playerTurn = false;
        var combatActive = true;
        
        if (!_enemy.IsBoss && _player.Data.HasItem(_bombItem.Key))
        {
            Instantiate(_bombAttackPrefab, _enemy.Model.transform.position, Quaternion.identity);
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
                _healthBarUI.SetFill(_enemy.CurrentHealth / (float)_enemy.MaxHealth);
                playerTurn = false;

                PlaySlashAttack(_enemy.Model.transform);
                GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"-{damage}", _enemy.Model.transform);
            }
            else
            {
                damage = Random.Range(_enemy.DamageMin, _enemy.DamageMax);
                damage = Mathf.Max(0, damage - _player.Data.Defense.Value);

                if (_player.Data.CurrentHealth.Value - damage <= 0 && _player.Data.HasItem(_reviveItem.Key))
                {
                    _player.Data.RemoveItem(_reviveItem.Key);
                    GameController.Instance.GameView.EventDetailDisplay.ShowMessage("Guardian Angel!");
                    var heal = Mathf.RoundToInt(_player.Data.MaxHealth * .5f);
                    _player.Data.CurrentHealth.Value += heal;
                }
                
                _player.Data.CurrentHealth.Value -= damage;
                playerTurn = true;
                
                PlaySlashAttack(_player.Model);

                var message = $"-{damage}";
                if (damage <= 0)
                    message = "Miss!";
                else
                    _impulseSource.GenerateImpulse(.05f);
               
                GameController.Instance.GameView.EventDetailDisplay.ShowMessage(message, _player.Model);
            }
            
            _audioSource.PlayOneShot(damage > 0 ? _hitSounds[Random.Range(0, _hitSounds.Length)] : _missSound, .35f);
            
            combatActive = _enemy.CurrentHealth > 0 && _player.Data.CurrentHealth.Value > 0;
            yield return new WaitForSeconds(combatActive ? .75f : .2f);
        }

        //player lost
        if (_player.Data.CurrentHealth.Value <= 0)
            yield break;
        
        //player won. Loss is handled with Player
        _player.MoveOutCombat();
        _player.Data.Experience.Value += _enemy.Experience;
        _player.Data.Gold.Value += 25 + (25 * GameController.Instance.MaxLoops);

        if (_enemy.IsBoss)
        {
            _audioSource.PlayOneShot(_bossDefeatSound, .5f);
            GameController.Instance.ChangeLevelLoop();
        }

        _healthBarUI.Hide();
        Destroy(_enemy.Model.gameObject);
        _enemy = null;
        
        yield return new WaitForSeconds(.2f);
        
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }

    private void PlaySlashAttack(Transform target)
    {
        var position = target.position;
        position.y += .5f;
        _slashAttack.transform.position = position;
        _slashAttack.Play();
    }
    
    private class Enemy
    {
        public int Experience => _enemyData.Experience;
        public bool IsBoss => _enemyData.IsBoss;
        public int MaxHealth { get; private set; }
        
        public int CurrentHealth;
        public readonly int DamageMin;
        public readonly int DamageMax;
        public GameObject Model;

        private readonly EnemyData _enemyData;
        
        public Enemy(EnemyData enemyData, int loops)
        {
            _enemyData = enemyData;
            MaxHealth = Mathf.RoundToInt(_enemyData.Health * Mathf.Pow(GameController.Instance.CombatExponentialValue, loops));
            CurrentHealth = MaxHealth;
            DamageMin = Mathf.RoundToInt(_enemyData.DamageMin * Mathf.Pow(GameController.Instance.CombatExponentialValue, loops));
            DamageMax = Mathf.RoundToInt(_enemyData.DamageMax * Mathf.Pow(GameController.Instance.CombatExponentialValue, loops));
            Debug.Log(_enemyData.DamageMin + " - " + _enemyData.DamageMax + " | " + DamageMin + " - " + DamageMax);
            Debug.Log(_enemyData.Health + " - " + CurrentHealth);
        }
    }
}