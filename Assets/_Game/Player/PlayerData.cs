using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public List<Item> Items = new List<Item>();
    
    public PlayerValue Experience;
    public PlayerValue Level;
    public PlayerValue Gold;
    public PlayerValue StatPoints;
    
    //stats
    public PlayerValue MaxHealth;
    public PlayerValue CurrentHealth;
    public PlayerValue Damage;
    public PlayerValue CriticalChance;
    public PlayerValue CriticalDamage;
    public PlayerValue Protection;
    public PlayerValue Evasion;

    //stats
    public int BossDefeated;
    public int TotalMoves;

    public void Initialize()
    {
        MaxHealth.OnValueChanged += value => { CurrentHealth.SetMaxValue(value); };
    }

    //todo change to gear
    /*
    private void ProcessItemBonuses(ItemData itemData, bool add)
    {
        foreach (var bonus in itemData.StatBonuses)
        {
            var value = add ? bonus.Amount : -bonus.Amount;
            switch (bonus.Type)
            {
                case StatType.MaxHealth:
                    MaxHealth.Value += value;
                    break;
                case StatType.CurrentHealth:
                    CurrentHealth.Value += value;
                    break;
                case StatType.Damage:
                    Damage.Value += value;
                    break;
                case StatType.Protection:
                    Protection.Value += value;
                    break;
                case StatType.Experience:
                    Experience.Value += value;
                    break;
                case StatType.CurrentHealthPercent:
                    var percent = bonus.Amount / 100f;
                    var total = Mathf.RoundToInt(MaxHealth.Value * percent);
                    CurrentHealth.Value += total;
                    break;
            }
        }
    }
    */
    
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
        //ProcessItemBonuses(itemData, true);
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
        //ProcessItemBonuses(itemToRemove.Data, false);
        OnItemRemoved?.Invoke(itemToRemove);
    }
    
    #endregion
}

public class PlayerValue
{
    public delegate void OnStatChangedDelegate(int newValue);
    public event OnStatChangedDelegate OnValueChanged;

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
            OnValueChanged?.Invoke(_value);
        }
    }
    
    public void SetMaxValue(int maxValue)
    {
        _maxValue = maxValue;
    }
}