using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
    public Item Item { get; private set; }
    
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemDescriptionText;
    [SerializeField] private Image _itemIcon;

    [SerializeField] protected Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(ButtonPressed);
    }

    public virtual void SetItem(Item item)
    {
        Item = item;
        
        _itemNameText.text = item.Data.Name;
        _itemDescriptionText.text = item.Data.Description;
        _itemIcon.sprite = item.Data.Icon;

        _button.enabled = !item.Data.DisableUse;
    }
    
    private void ButtonPressed()
    {
        foreach (var effect in Item.Data.Effects)
            effect.ApplyEffect();
        
        GameController.Instance.Player.Data.RemoveItem(Item.Data.Key);
    }
}