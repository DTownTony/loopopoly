using UnityEngine;

[CreateAssetMenu(fileName = "StatEvent", menuName = "Data/BoardEvent/StatEvent")]
public class StatEvent : BoardEvent
{
    [SerializeField] private StatType _type;
    [SerializeField] private int _amount;

    public override void Trigger()
    {
        base.Trigger();
        GameController.Instance.EventHandler.StatUpdate(_type, _amount);

        var type = _type.ToString();
        switch (_type)
        {
            case StatType.MaxHealth:
                type = "Max Health";
                break;
            case StatType.CurrentHealth:
                type = "Health";
                break;
            case StatType.Experience:
                type = "Exp";
                break;
        }
        var col1 = new Color32(52, 155, 242,255);
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage($"+{_amount} {type}", col:col1);
    }
}