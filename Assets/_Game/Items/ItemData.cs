using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item/ItemData")]
public class ItemData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public int Cost;
    
    public string Key;
}
