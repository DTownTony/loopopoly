using UnityEngine;

[CreateAssetMenu(fileName = "TokenEvent", menuName = "Data/BoardEvent/TokenEvent")]
public class TokenEvent : BoardEvent
{
    [SerializeField] private Color _textColor;
    
    public override void Trigger()
    {
        base.Trigger();
        GameController.Instance.AddToken(1);
        GameController.Instance.GameView.EventDetailDisplay.ShowMessage("+1 Token!", col: _textColor);
    }
}