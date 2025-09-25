using System;
using UnityEngine;

public class Gear
{
    public GearData Data { get; private set; }

    public Gear(GearData data)
    {
        Data = data;
    }
}
