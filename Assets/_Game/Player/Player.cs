using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void OnMovedSpaceDelegate(int movesLeft); //todo: use this for events 
    public event OnMovedSpaceDelegate OnMovedSpace;
    
    public int CurrentPositionIndex { get; private set; }
    public int MovesLeft { get; private set; }
    public Transform Model;
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _moveSound;
    [SerializeField] private AudioClip _loopSound;
    [SerializeField] private AudioClip _levelUpSound;
    
    public readonly PlayerData Data = new PlayerData
    {
        Level = new PlayerValue(0),
        Experience = new PlayerValue(0, 0, 100),
        Gold = new PlayerValue(100),
        MaxHealth = new PlayerValue(50),
        CurrentHealth = new PlayerValue(50, 0, 50),
        Damage = new PlayerValue(5, 1),
        CriticalChance = new PlayerValue(5),
        CriticalDamage = new PlayerValue(50),
        Evasion = new PlayerValue(0),
        Protection = new PlayerValue(0),
        StatPoints = new PlayerValue(0)
    };
    
    private void Start()
    {
        Data.Initialize();
        Data.CurrentHealth.OnValueChanged += CheckCurrentHealth;
        Data.Experience.OnValueChanged += ExperienceChanged;
    }

    #region Level / Experience
    
    private void ExperienceChanged(int newValue)
    {
        if (newValue < GetExperienceNeededForLevel()) 
            return;
        
        LevelUp();
    }

    private void LevelUp()
    {
        Data.Level.Value++;
        Data.Experience.Value = 0;

        Data.Experience.SetMaxValue(GetExperienceNeededForLevel());
        Data.CurrentHealth.Value = Data.MaxHealth.Value;
        Data.StatPoints.Value += 3;
        
        _audioSource.PlayOneShot(_levelUpSound, .5f);
        var col1 = new Color32(52, 155, 242,255);
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage("Level Up!", col:col1);
    }

    #endregion
    
    private void CheckCurrentHealth(int newValue)
    {
        if (newValue > 0)
            return;

        const string reviveItemKey = "guardian_angel";
        if (Data.HasItem(reviveItemKey))
        {
            Data.RemoveItem(reviveItemKey);
            //todo: play revive effect
            //_audioSource.PlayOneShot(_reviveSound, 1f);
            
            var heal = Mathf.RoundToInt(Data.MaxHealth.Value);
            Data.CurrentHealth.Value += heal;
            return;
        }
        
        GameController.Instance.ChangeCurrentState(GameState.Death);
    }
    
    public void Move(List<BoardPosition> positions, Action onComplete, bool movedForward)
    {
        Data.TotalMoves++;
        MovesLeft = positions.Count;
        StartCoroutine(MoveSequence(positions, onComplete, movedForward));
    }
    
    public void MoveForCombat()
    {
        Model.DOLocalMoveX(-1, .1f);
    }
    
    public void MoveOutCombat()
    {
        Model.DOLocalMoveX(0, .1f);
    }

    private IEnumerator MoveSequence(List<BoardPosition> positions, Action onComplete, bool movedForward)
    {
        const float moveDuration = .35f;
        var startY = Model.position.y;
        var endY = startY + .5f;
        foreach (var position in positions)
        {
            transform.DOMove(position.transform.position, moveDuration);
            Model.DOLocalMoveY(endY, moveDuration * .5f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(moveDuration);

            _audioSource.PlayOneShot(_moveSound, .35f);
            MovesLeft--;
            OnMovedSpace?.Invoke(MovesLeft);
            CurrentPositionIndex = position.Index;

            //loop completed
            if (CurrentPositionIndex == 0 && movedForward)
            {
                var controller = GameController.Instance;
                controller.IncreaseGameLoop();
                if (controller.BossFightAvailable())
                {
                    controller.EventHandler.SetupBossCombat();
                    MovesLeft = 0;
                    OnMovedSpace?.Invoke(MovesLeft);
                    yield break;
                }
                
                //loop gold
                var goldAmount = 100 + (50 * controller.MaxLoops);
                Data.Gold.Value += goldAmount;
                var col = new Color32(255, 220, 0,255);
                controller.GameView.EventDetailDisplay.ShowMessage($"Loop {GameController.Instance.MaxLoops}\nGold +{goldAmount}!",col:col);
                _audioSource.PlayOneShot(_loopSound, 1f);
            }
        }
        
        onComplete?.Invoke();
    }

    public int GetExperienceNeededForLevel()
    {
        return 50 + Mathf.RoundToInt(Data.Level.Value * 25f);
    }

    public void PlacePlayer(BoardPosition position)
    {
        var endY = Model.position.y + 5f;
        
        transform.DOMove(position.transform.position, 0f);
        Model.DOLocalMoveY(endY, 0.5f)
            .From()
            .SetDelay(0.25f)
            .OnComplete(()=>_audioSource.PlayOneShot(_moveSound, .35f));
        
        CurrentPositionIndex = position.Index;
    }
}