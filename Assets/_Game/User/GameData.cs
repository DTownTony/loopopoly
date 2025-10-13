using System.Collections.Generic;

public class GameData
{
    public int TokensToUnlock;
    public int Gems;
    
    public int LevelIndex = 0;
    public int BoardIndex = -1;

    public List<string> UnlockedItems = new List<string>();
    public List<string> UnlockedEvents = new List<string>();
}