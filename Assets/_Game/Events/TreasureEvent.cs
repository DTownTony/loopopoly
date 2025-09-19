using UnityEngine;

[CreateAssetMenu(fileName = "TreasureEvent", menuName = "Data/BoardEvent/TreasureEvent")]
public class TreasureEvent : BoardEvent
{
    public override void Trigger()
    {
        GameController.Instance.EventHandler.ShowTreasure();
    }
}