using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Data/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> Items;

    public ItemData GetRandomItem()
    {
        return Items[Random.Range(0, Items.Count)];
    }
}