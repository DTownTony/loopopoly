using UnityEngine;

[CreateAssetMenu(fileName = "ShopEvent", menuName = "Data/BoardEvent/ShopEvent")]
public class ShopEvent : BoardEvent
{
    [SerializeField] private ItemDatabase _itemDatabase;
    
    private const int TOTAL_ITEMS = 3;
    
    public override void Trigger()
    {
        var items = _itemDatabase.GetRandomItems(TOTAL_ITEMS, ItemType.Special);
        GameController.Instance.EventHandler.EventView.ShowItemsEvent(new ItemsEventUIArgs(
            Name, null, null, items));
    }
}