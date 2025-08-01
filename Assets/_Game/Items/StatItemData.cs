using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item/StatItemData")]
public class StatItemData : ItemData
{
    public List<StatBonus> StatBonuses = new List<StatBonus>();
}

[Serializable]
public class StatBonus
{
    public StatType Type;
    public int Amount;
}