using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Data/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> Items;

    public ItemData GetRandomItem(ItemType ignoreMask = ItemType.None)
    {
        var validItems = Items
            .Where(item => (ignoreMask & item.Type) == 0)
            .ToList();
        
        return validItems[Random.Range(0, validItems.Count)];
    }

    public List<ItemData> GetRandomItems(int amount, ItemType ignoreMask = ItemType.None)
    {
        var validItems = Items
            .Where(item => (ignoreMask & item.Type) == 0)
            .ToList();

        var itemsList = new List<ItemData>();
        for (var i = 0; i < amount; i++)
            itemsList.Add(validItems[Random.Range(0, validItems.Count)]);

        return itemsList;
    }
}