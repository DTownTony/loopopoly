using UnityEngine;

public class BoardPosition : MonoBehaviour
{
    public int Index { get; private set; }

    [SerializeField] private BoardEvent _event;
    protected GameObject _boardPiece;
    
    public void SetIndex(int index)
    {
        Index = index;
    }

    public void SetEvent(BoardEvent boardEvent, GameObject boardPiece)
    {
        _event = boardEvent;
        
        if (_boardPiece != null)
            Destroy(_boardPiece);
        
        var position = transform.position;
        position.y += (Random.Range(0, 3) * 0.05f);
        _boardPiece = Instantiate(boardPiece, position, transform.rotation, transform);
    }

    public void Trigger()
    {
        //Debug.Log("Landed on position: " + Index + " with event: " + _event.Name);
        _event.Trigger();
    }
}