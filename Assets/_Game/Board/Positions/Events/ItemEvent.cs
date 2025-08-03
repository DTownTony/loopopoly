using UnityEngine;

[CreateAssetMenu(fileName = "ItemEvent", menuName = "Data/BoardEvent/ItemEvent")]
public class ItemEvent : BoardEvent
{
    [SerializeField] private ItemData _itemToAdd;

    public override void Trigger()
    {
        base.Trigger();
        GameController.Instance.EventHandler.AddItem(_itemToAdd);
        var col1 = new Color(52, 155, 242);
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"+{_itemToAdd.Name}", col:col1);
    }
}