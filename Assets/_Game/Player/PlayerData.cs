using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int MaxHealth;
    public List<Item> Items = new List<Item>();
    
    public PlayerValue Experience;
    public PlayerValue Level;
    public PlayerValue Gold;
    public PlayerValue CurrentHealth;
    public PlayerValue Damage;
    public PlayerValue Defense;
    
    public void UpdateMaxHealth(int amount)
    {
        MaxHealth += amount;
        CurrentHealth.SetMaxValue(MaxHealth);
    }
    
    #region Items
    
    public delegate void OnItemDelegate(Item item);
    public event OnItemDelegate OnItemAdded;
    public event OnItemDelegate OnItemRemoved;
    
    public bool HasItem(string key)
    {
        return Items.Exists(item => item.Key == key);
    }
    
    public void AddItem(ItemData itemData)
    {
        var item = new Item(itemData);
        Items.Add(item);
        OnItemAdded?.Invoke(item);
    }
    
    public void RemoveItem(string key)
    {
        Item itemToRemove = null;
        foreach (var item in Items)
        {
            if (item.Key.Equals(key))
            {
                itemToRemove = item;
                break;
            }
        }

        if (itemToRemove == null) 
            return;
        
        Items.Remove(itemToRemove);
        OnItemRemoved?.Invoke(itemToRemove);
    }
    
    #endregion
}

public class PlayerValue
{
    public delegate void OnStatChangedDelegate(int newValue);
    public event OnStatChangedDelegate OnStatChanged;

    private readonly int _minValue;
    private int _maxValue;
    private int _value;

    public PlayerValue(int startingValue, int minValue = 0, int maxValue = int.MaxValue)
    {
        _value = startingValue;
        _minValue = minValue;
        _maxValue = maxValue;
    }
    
    public int Value
    {
        get => _value;
        set
        {
            _value = Mathf.Clamp(value, _minValue, _maxValue);
            OnStatChanged?.Invoke(_value);
        }
    }
    
    public void SetMaxValue(int maxValue)
    {
        _maxValue = maxValue;
    }
}