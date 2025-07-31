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
    }
}