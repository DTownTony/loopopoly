using UnityEngine;

[CreateAssetMenu(fileName = "ArmorerEvent", menuName = "Data/Global Events")]
public class GlobalEvents : ScriptableObject
{
    public BoardEvent StartEvent;
    
    public BoardEvent[] EventData;
    public BoardEvent[] SpecialEventData;
    
    public BoardEvent[] EmptyCornerEventData;
}