using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GearDatabase", menuName = "Data/GearDatabase")]
public class GearDatabase : ScriptableObject
{
    public List<GearData> Database;
    
    //todo: add editor find

    public List<GearData> GetLockedGear(List<string> unlocked)
    {
        var lockedGearList = new List<GearData>();
        foreach (var gearData in Database)
        {
            if(unlocked.Contains(gearData.Id))
                continue;
            
            lockedGearList.Add(gearData);
        }

        return lockedGearList;
    }
}
