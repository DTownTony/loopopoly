using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const int EXP_NEXT_LEVEL = 100;
    
    public int CurrentIndex { get; private set; }
    public Transform Model;

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
        
        //todo: death screen
        GameController.Instance.ReloadGame();
    }
    
    public void Move(List<BoardPosition> positions, Action onComplete)
    {
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
            CurrentIndex = position.Index;

            //loop completed
            if (CurrentIndex == 0)
            {
                var controller = GameController.Instance;
                controller.IncreaseGameLoop();
                if (controller.BossFightAvailable())
                {
                    controller.EventHandler.SetupBossCombat();
                    yield break;
                }
                
                //loop gold
                var goldAmount = 100 * controller.MaxLoops;
                Data.Gold.Value += goldAmount;
                controller.GameView.EventDetailDisplay.ShowMessage($"Loop {GameController.Instance.MaxLoops}\nGold +{goldAmount}!");
            }
        }
        
        onComplete?.Invoke();
    }
}