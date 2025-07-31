using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoardEvent/GoldEvent", fileName = "GoldEvent")]
public class GoldEvent : BoardEvent
{
    [SerializeField] private int _goldAmount;
    
    public override void Trigger()
    {
        base.Trigger();
        GameController.Instance.EventHandler.AddGold(_goldAmount);
    }
}