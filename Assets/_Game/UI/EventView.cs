using UnityEngine;

public class EventView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    
   
    [SerializeField] private InfoEventUI _infoEventUI;
    [SerializeField] private ItemsEventUI _itemsEventUI;
    [SerializeField] private ChoiceEventUI _choiceEventUI;
    [SerializeField] private SelectEventUI _selectEventUI;
    
    public void ShowSelectEvent(SelectEventUIArgs args)
    {
        _canvas.enabled = true;
        _selectEventUI.Show(args);
    }

    public void ShowInfoEvent(EventUIArgsBase args)
    {
        _canvas.enabled = true;
        _infoEventUI.Show(args);
    }

    public void ShowItemsEvent(ItemsEventUIArgs args)
    {
        _canvas.enabled = true;
        _itemsEventUI.Show(args);
    }

    public void ShowChoiceEvent(ChoiceEventUIArgs args)
    {
        _canvas.enabled = true;
        _choiceEventUI.Show(args);
    }

    public void Hide()
    {
        _canvas.enabled = false;
    }
}