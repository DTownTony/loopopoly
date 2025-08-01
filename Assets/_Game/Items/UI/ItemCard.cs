using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
    public Item Item;
    
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemDescriptionText;
    [SerializeField] private Image _itemIcon;

    public virtual void SetItem(Item item)
    {
        Item = item;
        
        _itemNameText.text = item.Data.Name;
        _itemDescriptionText.text = item.Data.Description;
        _itemIcon.sprite = item.Data.Icon;
    }
}