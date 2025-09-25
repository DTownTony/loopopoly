using System.Collections.Generic;
using UnityEngine;

public class GearInventory
{
    public List<Gear> Inventory = new List<Gear>();
    public Dictionary<string, Gear> Equipped = new Dictionary<string, Gear>();

    public void AddGear(GearData data)
    {
        var newGear = new Gear(data);
        Inventory.Add(newGear);
    }
}