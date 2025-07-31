using UnityEngine;

[CreateAssetMenu(fileName = "ItemEvent", menuName = "Data/BoardEvent/ItemEvent")]
public class ItemEvent : BoardEvent
{
    public ItemData ItemToAdd;

    public override void Trigger()
    {
        base.Trigger();
        GameController.Instance.EventHandler.AddItem(ItemToAdd);
    }
}