using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoardEvent/GoldEvent", fileName = "GoldEvent")]
public class GoldEvent : BoardEvent
{
    [SerializeField] private string[] _descriptions;
    [SerializeField] private int _goldAmount;
    
    public override void Trigger()
    {
        base.Trigger();
        GameController.Instance.EventHandler.AddGold(_goldAmount);
        //var col = new Color32(255, 220, 0,255);
        //GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"Gold +{_goldAmount}!", col: col);
        GameController.Instance.EventHandler.EventView.ShowInfoEvent(new EventUIArgsBase(Name,
            null, _descriptions[Random.Range(0, _descriptions.Length)]));
    }
}