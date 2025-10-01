using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item/ItemData")]
public class ItemData : ScriptableObject
{
    public string Id;
    public ItemType Type;
    
    public string Name;
    public string Description;
    public Sprite Icon;
    public int Cost;
    
    public string Key;
    
    public ItemEffect[] Effects;
}

[System.Flags]
public enum ItemType
{
    None = 0,
    Basic = 1 << 0,
    Special = 1 << 1,
    Gear = 1 << 2
}