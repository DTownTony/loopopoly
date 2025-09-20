using UnityEngine;

[CreateAssetMenu(menuName = "Data/BoardEvent/GoldEvent", fileName = "GoldEvent")]
public class GoldEvent : BoardEvent
{
    [SerializeField] private string[] _descriptions;
    [SerializeField] private int _goldAmount;
    [SerializeField] private Sprite _icon;

    [SerializeField] private Color _textColor;
    
    public override void Trigger()
    {
        GameController.Instance.EventHandler.AddGold(_goldAmount);
        
        var args = new EventUIArgsBase(Name,
            _icon, _descriptions[Random.Range(0, _descriptions.Length)]);
        
        args.OnHide += () =>
        {
            GameController.Instance.GameView.EventDetailDisplay.ShowMessage(
                _goldAmount > 0 ? $"Gold +{_goldAmount}!" : $"Gold {_goldAmount}", col: _textColor);
        };
        GameController.Instance.EventHandler.EventView.ShowInfoEvent(args);
    }
}