using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopEvent", menuName = "Data/BoardEvent/ShopEvent")]
public class ShopEvent : BoardEvent
{
    [SerializeField] private ItemDatabase _itemDatabase;
    
    private const int TOTAL_ITEMS = 3;
    
    public override void Trigger()
    {
        var items = new List<ItemData>();
        for (var i = 0; i < TOTAL_ITEMS; i++)
        {
            var randomItem = _itemDatabase.GetRandomItem();
            items.Add(randomItem);
        }
        
        GameController.Instance.EventHandler.EventView.ShowItemsEvent(new ItemsEventUIArgs(
            Name, null, null, items));
    }
}