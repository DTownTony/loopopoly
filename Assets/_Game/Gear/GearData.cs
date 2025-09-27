using UnityEngine;

[CreateAssetMenu(fileName = "GearData", menuName = "Data/Gear/GearData")]
public class GearData : ScriptableObject
{
    public GearType Type;
    
    public string Name;
    public Sprite Icon;
    
    //todo: gear effects
}
