using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private EventView _eventView;
    [SerializeField] private ShopItemCard _shopItemCardPrefab;
    [SerializeField] private Transform _itemContainer;
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private Button _closeButton;

    private readonly List<ShopItemCard> _items = new List<ShopItemCard>();
    
    private const int TOTAL_ITEMS = 3;

    private void Awake()
    {
        _closeButton.onClick.AddListener(CloseButtonPressed);
    }

    public void Show()
    { 
        gameObject.SetActive(true);

        for (var i = 0; i < TOTAL_ITEMS; i++)
        {
            var randomItemData = _itemDatabase.GetRandomItem();
            var itemCard = Instantiate(_shopItemCardPrefab, _itemContainer);
            itemCard.SetItem(new Item(randomItemData));
            itemCard.OnPurchase += Hide;
            _items.Add(itemCard);
        }
    }
    
    private void CloseButtonPressed()
    {
        Hide();
    }

    private void Hide()
    {
        foreach (var item in _items)
            Destroy(item.gameObject);
        _items.Clear();
        
        gameObject.SetActive(false);
        _eventView.Hide();
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }
}