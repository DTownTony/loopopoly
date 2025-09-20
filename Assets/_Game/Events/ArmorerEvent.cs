using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorerEvent", menuName = "Data/BoardEvent/ArmorerEvent")]
public class ArmorerEvent : BoardEvent
{
    [SerializeField] private List<ItemData> _items;
    
    public override void Trigger()
    {
        //todo
        GameController.Instance.EventHandler.EventView.ShowItemsEvent(
            new ItemsEventUIArgs(Name, null, null, _items));
    }
}