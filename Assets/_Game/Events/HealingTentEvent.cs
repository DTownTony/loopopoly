using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealingTentEvent", menuName = "Data/BoardEvent/HealingTentEvent")]
public class HealingTentEvent : BoardEvent
{
    [SerializeField] private List<ItemData> _items = new List<ItemData>();
    public override void Trigger()
    {
        //todo
        GameController.Instance.EventHandler.ShowItemsEvent(
            new ItemsEventUIArgs(Name, null, null, _items));
    }
}