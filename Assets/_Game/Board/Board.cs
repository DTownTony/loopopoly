using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private BoardPosition[] _boardPositions;

    private void Awake()
    {
        //initialize board positions
        for (var i = 0; i < _boardPositions.Length; i++)
        {
            var boardPosition = _boardPositions[i];
            boardPosition.SetIndex(i);
        }
    }

    public List<BoardPosition> GetBoardPositions(int starting, int move)
    {
        starting += 1;
        
        var positions = new List<BoardPosition>();
        var posIndex = 0;
        for (var i = 0; i < move; i++)
        {
            var index = starting + posIndex;
            if (index >= _boardPositions.Length)
            {
                //reset all
                starting = 0;
                posIndex = 0;
                index = 0;
            }
            
            var position = GetBoardPosition(index);
            positions.Add(position);
            posIndex++;
        }

        return positions;
    }
    
    private BoardPosition GetBoardPosition(int index)
    {
        if (index < 0 || index >= _boardPositions.Length)
            return null;
        
        return _boardPositions[index];
    }
}