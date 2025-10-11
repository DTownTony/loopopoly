using UnityEngine;

public class PlayerValueHealthUI : PlayerValueUI
{
    private PlayerValue _maxHeathValue;

    public void SetMaxHealth(PlayerValue maxHealthValue)
    {
        _maxHeathValue = maxHealthValue;
        _maxHeathValue.OnValueChanged += Set;
    }
    
    protected override void Set(int amount)
    {
        _amountText.SetText(amount + "/" + _maxHeathValue.Value);
    }
}