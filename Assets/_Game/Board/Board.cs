using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private BoardPosition[] _boardPositions;
    
    [SerializeField] private BoardEvent[] _eventData;
    [SerializeField] private BoardEvent[] _specialEventData;

    private readonly HashSet<int> _specialEventIndexes = new HashSet<int>()
    {
        10, 20, 30
    };

    private void Awake()
    {
        var availableSpecialEvents = new List<BoardEvent>(_specialEventData);
        //initialize board positions
        for (var i = 0; i < _boardPositions.Length; i++)
        {
            var boardPosition = _boardPositions[i];
            boardPosition.SetIndex(i);

            if (_specialEventIndexes.Contains(i))
            {
                var boardEvent = availableSpecialEvents[Random.Range(0, availableSpecialEvents.Count)];
                boardPosition.SetEvent(boardEvent);
                availableSpecialEvents.Remove(boardEvent);
            }
            else
            {
                boardPosition.SetEvent(_eventData[Random.Range(0, _eventData.Length)]);
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