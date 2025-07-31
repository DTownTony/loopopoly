using System;

public class Item
{
    public string Key { get; private set; }

    [NonSerialized] public ItemData Data;
    
    public Item(ItemData itemData)
    {
        Data = itemData;
        Key = itemData.Key;
    }
}
