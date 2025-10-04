using UnityEngine;

public class PlayerValueCritDamageUI : PlayerValueUI
{
    protected override void Set(int amount)
    {
        _amountText.SetText($"{amount}%");
    }
}