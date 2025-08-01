using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    [SerializeField] private ItemCard _itemCardPrefab;
    [SerializeField] private Transform _itemParent;

    private List<ItemCard> _cards = new List<ItemCard>();

    private void Start()
    {
        GameController.Instance.Player.Data.OnItemAdded += ItemAdded;
        GameController.Instance.Player.Data.OnItemRemoved += ItemRemoved;
    }
    
    private void ItemAdded(Item item)
    {
        var itemCard = Instantiate(_itemCardPrefab, _itemParent);
        itemCard.SetItem(item);
    }
    
    private void ItemRemoved(Item item)
    {
        var itemCard = _cards.Find(c => c.Item == item);
        if (itemCard != null)
        {
            _cards.Remove(itemCard);
            Destroy(itemCard.gameObject);
        }
    }
}