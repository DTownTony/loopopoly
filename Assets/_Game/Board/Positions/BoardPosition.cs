using UnityEngine;

public class BoardPosition : MonoBehaviour
{
    public int Index { get; private set; }

    [SerializeField] private BoardEvent _event;
    
    public void SetIndex(int index)
    {
        Index = index;
    }

    public void SetEvent(BoardEvent boardEvent)
    {
        _event = boardEvent;
    }

    public void Trigger()
    {
        Debug.Log("Landed on position: " + Index + " with event: " + _event.name);
        _event.Trigger();
    }
}