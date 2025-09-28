using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorerEvent", menuName = "Data/Global Events")]
public class GlobalEvents : ScriptableObject
{
    public BoardEvent StartEvent;
    
    public BoardEvent[] EventData;
    public BoardEvent[] SpecialEventData;
    public BoardEvent[] LockedEventData;
    
    public List<BoardEvent> GetLockedEvents(List<string> unlocked)
    {
        var lockedEvents = new List<BoardEvent>();
        foreach (var eventData in LockedEventData)
        {
            if (unlocked.Contains(eventData.Id))
                continue;
            
            lockedEvents.Add(eventData);
        }

        return lockedEvents;
    }
}