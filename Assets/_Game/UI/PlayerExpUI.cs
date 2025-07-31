using UnityEngine;
using UnityEngine.UI;

public class PlayerExpUI : PlayerValueUI
{
    [SerializeField] private Image _expFill;

    protected override void Set(int amount)
    {
        _amountText.SetText(amount + "/" + Player.EXP_NEXT_LEVEL);
        
        var percent = (float)amount / Player.EXP_NEXT_LEVEL;
        _expFill.fillAmount = percent;
    }
}