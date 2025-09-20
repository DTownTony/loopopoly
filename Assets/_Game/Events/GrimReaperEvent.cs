using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Data/BoardEvent/InstantDeath", fileName = "InstantDeath")]
public class GrimReaperEvent : BoardEvent
{
    [SerializeField] private Sprite _grimReaperSprite;
    [SerializeField] private ItemData _stopEventItem;
    
    [Header("Audio")]
    [SerializeField] private AudioClip _skipSound;
    [SerializeField] private AudioClip _hurtSound;

    [NonSerialized] private float _currentDeathChance;
    
    public override void Trigger()
    {
        _currentDeathChance = .25f;
        for (var i = 0; i < GameController.Instance.TotalLoopCount; i++)
            _currentDeathChance += Random.Range(.1f, .15f);

        GameController.Instance.EventHandler.EventView.ShowChoiceEvent(new ChoiceEventUIArgs(
            Name, _grimReaperSprite, "The grim reaper has come to visit!", 
            UseItem, TryLuck,
            $"Use {_stopEventItem.Name}!", 
            "Try My Luck: " + (_currentDeathChance * 100).ToString("F2") + "% chance of death",
            GameController.Instance.Player.Data.HasItem(_stopEventItem.Key)));
    }

    private void UseItem()
    {
        GameController.Instance.Player.Data.RemoveItem(_stopEventItem.Key);
        //_audioSource.PlayOneShot(_skipSound, 1f);
    }

    private void TryLuck()
    {
        if (Random.value >= _currentDeathChance)
        {
            GameController.Instance.GameView.EventDetailDisplay.ShowMessage("Lucky!",
                col: new Color32(52, 155, 242, 255));
            //_audioSource.PlayOneShot(_skipSound, 1f);
        }
        else
        {
            var player = GameController.Instance.Player;
            var damage = Mathf.RoundToInt(player.Data.CurrentHealth.Value * .9f);
            player.Data.CurrentHealth.Value -= damage;
            //_audioSource.PlayOneShot(_hurtSound, 1f);
        }
    }
}