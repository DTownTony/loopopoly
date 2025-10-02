using System;

public class Item
{
    public string Id { get; private set; }

    [NonSerialized] public ItemData Data;
    
    public Item(ItemData itemData)
    {
        Data = itemData;
        Id = itemData.Id;
    }
}
