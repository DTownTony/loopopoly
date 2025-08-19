using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private BoardPosition[] _boardPositions;
    [SerializeField] private GlobalEvents _globalEvents;

    [SerializeField] private List<int> _specialEventIndexes = new List<int>();

    public void BuildBoard()
    {
        if (_boardPositions == null || _boardPositions.Length == 0)
        {
            Debug.LogWarning("[Board] No board positions to build.");
            return;
        }
        
        var availableEvents = new List<BoardEvent>(_globalEvents.EventData);
        var specialEvents = new List<BoardEvent>(_globalEvents.SpecialEventData);
        specialEvents.AddRange(_globalEvents.EmptyCornerEventData);
        
        for (var i = 0; i < _boardPositions.Length; i++)
        {
            var boardPosition = _boardPositions[i];
            boardPosition.SetIndex(i);

            //todo: weighted events
            if (i == 0)
            {
                var boardEvent = _globalEvents.EmptyCornerEventData[0]; //todo: go piece
                var piece = boardEvent.BoardPiece[0];  //todo: refactor piece
                boardPosition.SetEvent(boardEvent, piece);
                continue;
            }
            
            var isCorner = _specialEventIndexes.Contains(i);
            if (isCorner)
            {
                var boardEvent = specialEvents[Random.Range(0, specialEvents.Count)];

                var piece = boardEvent.BoardPiece[Random.Range(0, boardEvent.BoardPiece.Length)];
                boardPosition.SetEvent(boardEvent, piece);
            }
            else
            {
                var boardEvent = availableEvents[Random.Range(0, availableEvents.Count)];

                var piece = boardEvent.BoardPiece[Random.Range(0, boardEvent.BoardPiece.Length)];
                boardPosition.SetEvent(boardEvent, piece);
            }
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
                // reset all
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

    public BoardPosition GetBoardPosition(int index)
    {
        if (index < 0 || index >= _boardPositions.Length)
            return null;

        return _boardPositions[index];
    }
    

    // ----------------- helpers -----------------
    private static void Shuffle<T>(List<T> list)
    {
        // Fisher-Yates using UnityEngine.Random
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
