using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const int EXP_NEXT_LEVEL = 100;
    
    public int CurrentIndex { get; private set; }

    public readonly PlayerData Data = new PlayerData
    {
        Level = new PlayerValue(0),
        Experience = new PlayerValue(0, 0, EXP_NEXT_LEVEL),
        Gold = new PlayerValue(0),
        MaxHealth = 50,
        CurrentHealth = new PlayerValue(50, 0, 50),
        Damage = new PlayerValue(5, 1),
        Defense = new PlayerValue(0),
    };
    
    [SerializeField] private Transform _model;

    private Coroutine _moveRoutine;
    
    private void Start()
    {
        Data.Experience.OnStatChanged += ExperienceChanged;
    }

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
    }

    public void Move(List<BoardPosition> positions, Action onComplete)
    {
        _moveRoutine = StartCoroutine(MoveSequence(positions, onComplete));
    }
    
    public void MoveForCombat()
    {
        _model.DOLocalMoveX(-1, .1f);
    }
    
    public void MoveOutCombat()
    {
        _model.DOLocalMoveX(0, .1f);
    }

    private IEnumerator MoveSequence(List<BoardPosition> positions, Action onComplete)
    {
        const float moveDuration = .35f;
        var startY = _model.position.y;
        var endY = startY + .5f;
        foreach (var position in positions)
        {
            transform.DOMove(position.transform.position, moveDuration);
            _model.DOLocalMoveY(endY, moveDuration * .5f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(moveDuration);
            CurrentIndex = position.Index;

            if (CurrentIndex == 0)
            {
                var controller = GameController.Instance;
                controller.IncreaseGameLoop();
                if (controller.BossFightAvailable())
                {
                    controller.EventHandler.SetupBossCombat();
                    yield break;
                }
            }
        }
        
        onComplete?.Invoke();
    }
}