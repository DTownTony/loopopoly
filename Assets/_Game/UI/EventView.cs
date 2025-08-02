using UnityEngine;

public class EventView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ShopUI _shop;
    [SerializeField] private GrimReaperUI _grimReaper;
    [SerializeField] private TreasureUI _treasureUI;

    //todo: make base class for all event views
    
    public void ShowShop()
    {
        _canvas.enabled = true;
        _shop.Show();
    }

    public void ShowGrimReaper(GrimReaperUIArgs args)
    {
        _canvas.enabled = true;
        _grimReaper.Show(args);
    }
    
    public void ShowTreasure()
    {
        _canvas.enabled = true;
        _treasureUI.Show();
    }

    public void Hide()
    {
        _canvas.enabled = false;
    }
}