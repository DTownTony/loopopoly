using UnityEngine;

public class EventView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private ShopUI _shop;
    [SerializeField] private GrimReaperUI _grimReaper;
    [SerializeField] private TreasureUI _treasureUI;
    [SerializeField] private KingsTaxesUI _kingsTaxesUI;
    [SerializeField] private EventUI _basicEventUI;

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
    
    public void ShowKingsTaxes(KingsTaxesUIArgs args)
    {
        _canvas.enabled = true;
        _kingsTaxesUI.Show(args);
    }

    public void ShowHealingTent()
    {
        _canvas.enabled = true;
        
    }

    public void ShowBasicEvent(EventUIArgs args)
    {
        _canvas.enabled = true;
        _basicEventUI.Show(args);
    }

    public void Hide()
    {
        _canvas.enabled = false;
    }
}