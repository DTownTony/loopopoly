using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCard : ItemCard
{
    public event Action OnPurchase;
    
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Button _buyButton;
    
    private int _cost;

    private void Awake()
    {
        _buyButton.onClick.AddListener(BuyButtonPressed);
    }

    public override void SetItem(Item item)
    {
        base.SetItem(item);
        
        _cost = Mathf.RoundToInt(item.Data.Cost * Mathf.Pow(GameController.LOOP_EXPONENTIAL_VALUE, GameController.Instance.MaxLoops));
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