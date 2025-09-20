using System;
using TMPro;
using UnityEngine;

public class ShopItemCard : ItemCard
{
    public event Action OnPurchase;
    
    [SerializeField] private TMP_Text _costText;
    
    private int _cost;
    
    public override void SetItem(Item item)
    {
        base.SetItem(item);
        
        _cost = Mathf.RoundToInt(item.Data.Cost * Mathf.Pow(GameController.Instance.LoopExponentialValue, GameController.Instance.TotalLoopCount));
        _costText.SetText(_cost.ToString());
    }
    
    protected override void ButtonPressed()
    {
        if (GameController.Instance.Player.Data.Gold.Value < _cost) 
            return;
        
        GameController.Instance.Player.Data.Gold.Value -= _cost;
        GameController.Instance.Player.Data.AddItem(Item.Data);
        OnPurchase?.Invoke();
        Destroy(gameObject);
    }
}