using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackSmithEvent", menuName = "Data/BoardEvent/BlackSmithEvent")]
public class BlackSmithEvent : BoardEvent
{
    [SerializeField] private List<ItemData> _items;
    
    public override void Trigger()
    {
        //todo
        GameController.Instance.EventHandler.EventView.ShowItemsEvent(
            new ItemsEventUIArgs(Name, null, null, _items));
    }
}