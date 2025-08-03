using UnityEngine;

[CreateAssetMenu(menuName = "Data/Events/BoardEvent", fileName = "Empty Event")]
public class BoardEvent : ScriptableObject
{
    public string Name;
    public GameObject[] BoardPiece;
    public bool SpawnOneTime;
    
    public virtual void Trigger()
    {
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }
}