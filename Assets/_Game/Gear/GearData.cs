using UnityEngine;

[CreateAssetMenu(fileName = "GearData", menuName = "Data/Gear/GearData")]
public class GearData : ItemData
{
    [Header("Gear")]
    public GearType GearType;
    //todo: gear effects

}
