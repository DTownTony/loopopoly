using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoardEvent/GoldEvent", fileName = "GoldEvent")]
public class GoldEvent : BoardEvent
{
    [SerializeField] private int _goldAmount;
    
    public override void Trigger()
    {
        base.Trigger();
        var finalAmount =
            Mathf.RoundToInt(_goldAmount * Mathf.Pow(GameController.LOOP_EXPONENTIAL_VALUE, GameController.Instance.MaxLoops));
        
        GameController.Instance.EventHandler.AddGold(finalAmount);
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"Gold +{finalAmount}!");
    }
}