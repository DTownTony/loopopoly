using UnityEngine;

[CreateAssetMenu(fileName = "ItemEvent", menuName = "Data/BoardEvent/ItemEvent")]
public class ItemEvent : BoardEvent
{
    [SerializeField] private ItemData _itemToAdd;

    public override void Trigger()
    {
        base.Trigger();
        GameController.Instance.EventHandler.AddItem(_itemToAdd);
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"+{_itemToAdd.Name}");
    }
}