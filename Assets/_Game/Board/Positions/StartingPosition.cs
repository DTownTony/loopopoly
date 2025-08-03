using UnityEngine;
using Random = UnityEngine.Random;

public class StartingPosition : BoardPosition
{
    [SerializeField] private GameObject _bossTile;

    private GameObject _bossPiece;
    
    private void Start()
    {
        GameController.Instance.OnLoopsChanged += LoopsChanged;
    }
    
    private void LoopsChanged(int loops, int maxLoops)
    {
        var loopsLeft = maxLoops - loops;
        if (loopsLeft == 0)
        {
            _boardPiece.SetActive(false);

            var position = transform.position;
            position.y += (Random.Range(0, 3) * 0.05f);
            _bossPiece = Instantiate(_bossTile, position, transform.rotation, transform);
        } 
        else if (loopsLeft > 0 && _bossPiece != null)
        {
            Destroy(_bossPiece);
            _bossPiece = null;
            _boardPiece.SetActive(true);
        }
    }
}