using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StatEvent", menuName = "Data/BoardEvent/StatEvent")]
public class StatEvent : BoardEvent
{
    [SerializeField] private StatType _type;
    [SerializeField] private int _amount;

    public override void Trigger()
    {
        base.Trigger();
        GameController.Instance.EventHandler.StatUpdate(_type, _amount);

        switch (_type)
        {
            case StatType.MaxHealth:
                break;
            case StatType.CurrentHealth:
                break;
            case StatType.Damage:
                break;
            case StatType.Defense:
                break;
            case StatType.Experience:
                break;
        }
        
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"+{_amount} {_type}");
    }
}