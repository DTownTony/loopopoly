using UnityEngine;
using UnityEngine.UI;

public class PlayerExpUI : PlayerValueUI
{
    [SerializeField] private Image _expFill;

    protected override void Set(int amount)
    {
        var expNeededForLevel = GameController.Instance.Player.GetExperienceNeededForLevel();
        _amountText.SetText(amount + "/" + expNeededForLevel);
        
        var percent = (float)amount / expNeededForLevel;
        _expFill.fillAmount = percent;
    }
}