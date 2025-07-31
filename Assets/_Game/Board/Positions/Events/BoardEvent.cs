using UnityEngine;

[CreateAssetMenu(menuName = "Board/Events/BoardEvent", fileName = "Empty Event")]
public class BoardEvent : ScriptableObject
{
    public string Name;
    
    public virtual void Trigger()
    {
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }
}