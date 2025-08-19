using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private BoardPosition[] _boardPositions;
    [SerializeField] private BoardEvent[] _eventData;
    [SerializeField] private BoardEvent[] _specialEventData;
    [SerializeField] private BoardEvent[] _emptyCornerEventData;

    public List<int> _specialEventIndexes = new List<int>();

    public void BuildBoard()
    {
        if (_boardPositions == null || _boardPositions.Length == 0)
        {
            Debug.LogWarning("[Board] No board positions to build.");
            return;
        }

        // Work lists
        var availableEvents = new List<BoardEvent>(_eventData ?? new BoardEvent[0]);

        // Corner indices, excluding start index 0 (you handle start separately below)
        var cornerIndexes = new List<int>();
        for (var i = 0; i < _specialEventIndexes.Count; i++)
        {
            var idx = _specialEventIndexes[i];
            if (idx > 0 && idx < _boardPositions.Length) cornerIndexes.Add(idx);
        }
        Shuffle(cornerIndexes);

        // Special pool (unique use). If you use a special at start, we’ll exclude that entry from the pool.
        var uniqueSpecialPool = new List<BoardEvent>(_specialEventData ?? Array.Empty<BoardEvent>());
        Shuffle(uniqueSpecialPool);

        // Build loop
        for (var i = 0; i < _boardPositions.Length; i++)
        {
            var boardPosition = _boardPositions[i];
            boardPosition.SetIndex(i);

            // START TILE (index 0)
            if (i == 0)
            {
                // Prefer: a specific special (your previous behavior), else fall back
                BoardEvent startEvent = null;

                if (_specialEventData != null && _specialEventData.Length > 0)
                {
                    startEvent = _specialEventData[_specialEventData.Length - 1]; // your original choice (^1)
                    // remove this from the unique pool (avoid duplicates on corners)
                    RemoveFirstOccurrence(uniqueSpecialPool, startEvent);
                }
                else if (_emptyCornerEventData != null && _emptyCornerEventData.Length > 0)
                {
                    startEvent = _emptyCornerEventData[Random.Range(0, _emptyCornerEventData.Length)];
                }
                else if (_eventData != null && _eventData.Length > 0)
                {
                    startEvent = _eventData[Random.Range(0, _eventData.Length)];
                }

                if (startEvent != null)
                {
                    var piece = startEvent.BoardPiece[Random.Range(0, startEvent.BoardPiece.Length)];
                    boardPosition.SetEvent(startEvent, piece);
                }
                else
                {
                    Debug.LogWarning("[Board] No event data available for start tile.");
                }

                continue;
            }

            // CORNER TILE?
            var isCorner = _specialEventIndexes.Contains(i);
            if (isCorner)
            {
                BoardEvent boardEvent = null;

                if (uniqueSpecialPool.Count > 0)
                {
                    //Use each special once
                    var last = uniqueSpecialPool.Count - 1;
                    boardEvent = uniqueSpecialPool[last];
                    uniqueSpecialPool.RemoveAt(last);
                }
                else if (_emptyCornerEventData != null && _emptyCornerEventData.Length > 0)
                {
                    //Fill remaining corners with random empty corner events
                    boardEvent = _emptyCornerEventData[Random.Range(0, _emptyCornerEventData.Length)];
                }
                else
                {
                    //Fallbacks to avoid null refs if emptyCornerEventData is empty
                    if (_specialEventData != null && _specialEventData.Length > 0)
                        boardEvent = _specialEventData[Random.Range(0, _specialEventData.Length)];
                    else if (_eventData != null && _eventData.Length > 0)
                        boardEvent = _eventData[Random.Range(0, _eventData.Length)];
                }

                if (boardEvent != null)
                {
                    var piece = boardEvent.BoardPiece[Random.Range(0, boardEvent.BoardPiece.Length)];
                    boardPosition.SetEvent(boardEvent, piece);
                }
                else
                {
                    Debug.LogWarning($"[Board] No event available for corner at index {i}.");
                }
            }
            else
            {
                // NORMAL TILE
                if (availableEvents.Count == 0)
                {
                    // Refill from source as a fallback so we never crash
                    if (_eventData != null && _eventData.Length > 0)
                        availableEvents.AddRange(_eventData);
                    else
                    {
                        Debug.LogWarning($"[Board] No normal events available for index {i}.");
                        continue;
                    }
                }

                var boardEvent = availableEvents[Random.Range(0, availableEvents.Count)];
                if (boardEvent.SpawnOneTime)
                    availableEvents.Remove(boardEvent);

                var piece = boardEvent.BoardPiece[Random.Range(0, boardEvent.BoardPiece.Length)];
                boardPosition.SetEvent(boardEvent, piece);
            }
        }

        PlacePieces();
    }

    private void PlacePieces()
    {
        // Place Player at the start position
        GameController.Instance.Player.PlacePlayer(_boardPositions[0]);
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

    private BoardPosition GetBoardPosition(int index)
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

    private static void RemoveFirstOccurrence<T>(List<T> list, T item)
    {
        if (list == null) return;
        var idx = list.IndexOf(item);
        if (idx >= 0) list.RemoveAt(idx);
    }
    
#if UNITY_EDITOR
    public void SetSpecialEventIndexes(int index)
    { 
        _specialEventIndexes.Add(index);
    }
#endif
}
