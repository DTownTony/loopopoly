using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour
{
    [SerializeField] private EventView _eventView;
    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private Transform _container;
    [SerializeField] private Button _skipButton;
    
    [SerializeField] private ShopItemCard _itemCardPrefab;
    
    private readonly List<ShopItemCard> _items = new List<ShopItemCard>();

    private void Awake()
    {
        _skipButton.onClick.AddListener(SkipButtonPressed);
    }
    
    public void Show(EventUIArgs args)
    {
        _headerText.SetText(args.Title);
        gameObject.SetActive(true);
        
        for (var i = 0; i < args.Items.Count; i++)
        {
            var itemCard = Instantiate(_itemCardPrefab, _container);
            itemCard.SetItem(new Item(args.Items[i]));
            itemCard.OnPurchase += Hide;
            _items.Add(itemCard);
        }
    }
    
    private void SkipButtonPressed()
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

public class EventUIArgs
{
    public string Title;
    public List<ItemData> Items = new List<ItemData>();
}
