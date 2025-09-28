using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int TokensToUnlock;
    public int Gems;
    
    public int LevelIndex = 0;
    public int BoardIndex = -1;

    public List<int> UnlockedGear;
    public List<int> UnlockedEvents;
}