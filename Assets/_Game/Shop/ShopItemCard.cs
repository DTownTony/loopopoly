using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCard : ItemCard
{
    public event Action OnPurchase;
    
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Button _buyButton;

    private void Awake()
    {
        _buyButton.onClick.AddListener(BuyButtonPressed);
    }

    public override void SetItem(Item item)
    {
        base.SetItem(item);
        _costText.SetText(item.Data.Cost.ToString());
    }
    
    private void BuyButtonPressed()
    {
        if (GameController.Instance.Player.Data.Gold.Value >= Item.Data.Cost)
        {
            GameController.Instance.Player.Data.Gold.Value -= Item.Data.Cost;
            GameController.Instance.Player.Data.AddItem(Item.Data);
            OnPurchase?.Invoke();
            Destroy(gameObject);
        }
    }
}
