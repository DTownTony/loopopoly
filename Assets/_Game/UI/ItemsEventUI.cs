using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsEventUI : EventUIBase<ItemsEventUIArgs>
{
    [SerializeField] private Transform _container;
    [SerializeField] private Button _skipButton;
    
    [SerializeField] private ShopItemCard _itemCardPrefab;
    
    private readonly List<ShopItemCard> _items = new List<ShopItemCard>();

    private void Awake()
    {
        _skipButton.onClick.AddListener(SkipButtonPressed);
    }

    public override void Show(ItemsEventUIArgs args)
    {
        base.Show(args);
        
        for (var i = 0; i < args.Items.Count; i++)
        {
            var itemCard = Instantiate(_itemCardPrefab, _container);
            itemCard.SetItem(new Item(args.Items[i]));
            itemCard.OnPurchase += Purchased;
            _items.Add(itemCard);
        }
    }
    
    private void SkipButtonPressed()
    {
        Hide();
    }

    private void Purchased()
    {
        //.PlayOneShot(_goldSound, 1f);
        Hide();
    }

    protected override void Hide()
    {
        foreach (var item in _items)
            Destroy(item.gameObject);
        _items.Clear();
        
        base.Hide();
    }
}

public class ItemsEventUIArgs : EventUIArgsBase
{
    public readonly List<ItemData> Items;

    public ItemsEventUIArgs(string header, Sprite icon, string description, List<ItemData> items) : base(header, icon, description)
    {
        Items = items;
    }
}
