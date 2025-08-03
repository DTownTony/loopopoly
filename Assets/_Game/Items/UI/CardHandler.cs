using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    [SerializeField] private ItemCard _itemCardPrefab;
    [SerializeField] private Transform _itemParent;

    private readonly List<ItemCard> _cards = new List<ItemCard>();

    private void Start()
    {
        GameController.Instance.Player.Data.OnItemAdded += ItemAdded;
        GameController.Instance.Player.Data.OnItemRemoved += ItemRemoved;
    }
    
    private void ItemAdded(Item item)
    {
        var itemCard = Instantiate(_itemCardPrefab, _itemParent);
        itemCard.transform.SetAsFirstSibling();
        itemCard.SetItem(item);
        _cards.Add(itemCard);
    }
    
    private void ItemRemoved(Item item)
    {

        ItemCard itemCardToDestroy = null;
        foreach (var card in _cards)
        {
            if (card.Item == item)
            {
                itemCardToDestroy = card;
                break;
            }
        }

        if (itemCardToDestroy != null)
        {
            Destroy(itemCardToDestroy.gameObject);
            _cards.Remove(itemCardToDestroy);
        }
    }
}