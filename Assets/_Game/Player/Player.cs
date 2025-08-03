using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void OnMovedSpaceDelegate(int movesLeft); //todo: use this for events 
    public event OnMovedSpaceDelegate OnMovedSpace;
    
    public const int EXP_NEXT_LEVEL = 100;
    
    public int CurrentPositionIndex { get; private set; }
    public int MovesLeft { get; private set; }
    public Transform Model;
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _moveSound;
    
    public readonly PlayerData Data = new PlayerData
    {
        Level = new PlayerValue(0),
        Experience = new PlayerValue(0, 0, EXP_NEXT_LEVEL),
        Gold = new PlayerValue(100),
        MaxHealth = 50,
        CurrentHealth = new PlayerValue(50, 0, 50),
        Damage = new PlayerValue(5, 1),
        Defense = new PlayerValue(0),
    };
    
    private void Start()
    {
        Data.CurrentHealth.OnStatChanged += CheckCurrentHealth;
        Data.Experience.OnStatChanged += ExperienceChanged;
    }

    #region Level / Experience
    
    private void ExperienceChanged(int newValue)
    {
        if (newValue < EXP_NEXT_LEVEL) 
            return;
        
        LevelUp();
    }

    private void LevelUp()
    {
        Data.Level.Value++;
        Data.Experience.Value = 0;
        Data.UpdateMaxHealth(5);
        Data.Damage.Value++;
        Data.CurrentHealth.Value = Data.MaxHealth;
        
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage("Level Up!");
    }

    #endregion
    
    private void CheckCurrentHealth(int newValue)
    {
        if (newValue > 0)
            return;
        
        GameController.Instance.ChangeCurrentState(GameState.Death);
    }
    
    public void Move(List<BoardPosition> positions, Action onComplete)
    {
        Data.TotalMoves++;
        MovesLeft = positions.Count;
        StartCoroutine(MoveSequence(positions, onComplete));
    }
    
    public void MoveForCombat()
    {
        Model.DOLocalMoveX(-1, .1f);
    }
    
    public void MoveOutCombat()
    {
        Model.DOLocalMoveX(0, .1f);
    }

    private IEnumerator MoveSequence(List<BoardPosition> positions, Action onComplete)
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
            if (CurrentPositionIndex == 0)
            {
                var controller = GameController.Instance;
                controller.IncreaseGameLoop();
                if (controller.BossFightAvailable())
                {
                    controller.EventHandler.SetupBossCombat();
                    yield break;
                }
                
                //loop gold
                var goldAmount = 100 + (50 * controller.MaxLoops);
                Data.Gold.Value += goldAmount;
                controller.GameView.EventDetailDisplay.ShowMessage($"Loop {GameController.Instance.MaxLoops}\nGold +{goldAmount}!");
            }
        }
        
        onComplete?.Invoke();
    }
}