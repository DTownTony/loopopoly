using System;
using TMPro;
using UnityEngine;

public class ShopItemCard : ItemCard
{
    public event Action OnPurchase;
    
    [SerializeField] private TMP_Text _costText;
    
    private int _cost;

    private void Awake()
    {
        _button.onClick.AddListener(BuyButtonPressed);
    }

    public override void SetItem(Item item)
    {
        base.SetItem(item);
        
        _cost = Mathf.RoundToInt(item.Data.Cost * Mathf.Pow(GameController.Instance.LoopExponentialValue, GameController.Instance.MaxLoops));
        _costText.SetText(_cost.ToString());
    }
    
    private void BuyButtonPressed()
    {
        if (GameController.Instance.Player.Data.Gold.Value >= _cost)
        {
            GameController.Instance.Player.Data.Gold.Value -= _cost;
            GameController.Instance.Player.Data.AddItem(Item.Data);
            OnPurchase?.Invoke();
            Destroy(gameObject);
        }
    }
}