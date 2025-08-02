using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackSmithEvent", menuName = "Data/BoardEvent/BlackSmithEvent")]
public class BlackSmithEvent : BoardEvent
{
    [SerializeField] private List<ItemData> _items;
    
    public override void Trigger()
    {
        GameController.Instance.EventHandler.ShowBasicEvent(new EventUIArgs()
        {
            Title = Name,
            Items = new List<ItemData>(_items)
        });
    }
}