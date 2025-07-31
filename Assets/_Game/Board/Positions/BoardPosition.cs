using UnityEngine;

public class BoardPosition : MonoBehaviour
{
    public int Index { get; private set; }
    
    public void SetIndex(int index)
    {
        Index = index;
    }

    public void Trigger()
    {
        Debug.Log("Landed on position: " + Index);
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }
}