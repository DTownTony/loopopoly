public class PlayerLevelUI : PlayerValueUI
{
    protected override void Set(int amount)
    {
        _amountText.SetText((amount + 1).ToString());
    }
}
