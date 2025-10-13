using UnityEngine;

[CreateAssetMenu(fileName = "ItemEvent", menuName = "Data/BoardEvent/ItemEvent")]
public class ItemEvent : BoardEvent
{
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private string[] _descriptionText;

    public override void Trigger()
    {
        var item = _itemDatabase.GetRandomItem();
        
        GameController.Instance.EventHandler.AddItem(item);
        
        var description = string.Format(_descriptionText[Random.Range(0, _descriptionText.Length)], item.Name);
        GameController.Instance.EventHandler.EventView.ShowInfoEvent(new EventUIArgsBase(Name, item.Icon, description));
    }
}