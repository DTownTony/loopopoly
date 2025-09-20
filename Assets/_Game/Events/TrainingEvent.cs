using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrainingEvent", menuName = "Data/BoardEvent/TrainingEvent")]
public class TrainingEvent : BoardEvent
{
    [SerializeField] private List<ItemData> _items;
    
    public override void Trigger()
    {
        //todo
        GameController.Instance.EventHandler.EventView.ShowItemsEvent(
            new ItemsEventUIArgs(Name, null, null, _items));
    }
}