using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingTentEvent", menuName = "Data/BoardEvent/HealingTentEvent")]
public class HealingTentEvent : BoardEvent
{
    [SerializeField] private List<ItemData> _items = new List<ItemData>();
    public override void Trigger()
    {
        GameController.Instance.EventHandler.ShowBasicEvent(new EventUIArgs()
        {
            Title = Name,
            Items = new List<ItemData>(_items),
        });
    }
}