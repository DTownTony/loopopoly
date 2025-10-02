using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Data/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> Items;

    //todo: make this a loot table
    [SerializeField] private List<ItemData> _lockedItems;

    public ItemData GetRandomItem(ItemType ignoreMask = ItemType.None)
    {
        var validItems = Items
            .Where(item => (ignoreMask & item.Type) == 0)
            .ToList();

        //add locked items
        var unlockedItems = GlobalManagers.Instance.GameProfile.GameData.UnlockedItems;
        validItems.AddRange(_lockedItems.Where(item => unlockedItems.Contains(item.Id)));

        return validItems[Random.Range(0, validItems.Count)];
    }

    public List<ItemData> GetRandomItems(int amount, ItemType ignoreMask = ItemType.None)
    {
        var validItems = Items
            .Where(item => (ignoreMask & item.Type) == 0)
            .ToList();
        
        //add locked items
        var unlockedItems = GlobalManagers.Instance.GameProfile.GameData.UnlockedItems;
        validItems.AddRange(_lockedItems.Where(item => unlockedItems.Contains(item.Id)));

        var itemsList = new List<ItemData>();
        for (var i = 0; i < amount; i++)
            itemsList.Add(validItems[Random.Range(0, validItems.Count)]);

        return itemsList;
    }

    public List<ItemData> GetLockedItems()
    {
        var unlockedItems = GlobalManagers.Instance.GameProfile.GameData.UnlockedItems;
        var lockedItems = new List<ItemData>();
        foreach (var itemData in _lockedItems)
        {
            if(!unlockedItems.Contains(itemData.Id))
                lockedItems.Add(itemData);
        }

        return lockedItems;
    }
}