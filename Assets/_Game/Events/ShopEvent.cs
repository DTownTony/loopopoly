using UnityEngine;

[CreateAssetMenu(fileName = "ShopEvent", menuName = "Data/BoardEvent/ShopEvent")]
public class ShopEvent : BoardEvent
{
    public override void Trigger()
    {
        GameController.Instance.EventHandler.ShowShop();
    }
}