using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/MoveToItemEffect")]
public class MoveToItemEffect : ItemEffect
{
    [SerializeField] private BoardEvent _eventToMoveTo;
    [SerializeField] private bool _disableGoBack;
    
    public override void ApplyEffect()
    {
        var currentPosition = GameController.Instance.Player.CurrentPositionIndex;
        var position = GetBoardPosition(currentPosition, out var distance);
        
        GameController.Instance.PlayerMove(distance);
    }

    private BoardPosition GetBoardPosition(int currentPosition, out int distanceToMove)
    {
        var eventPositions = new List<BoardPosition>();

        var allPositions = GameController.Instance.Board.GetBoardPositions(); //reverse so forward is prioritized 
        foreach (var position in allPositions)
        {
            if (position.IsSameEvent(_eventToMoveTo))
                eventPositions.Add(position);
        }
        
        //find nearest position
        var closestDistance = int.MaxValue;
        BoardPosition closestPosition = null;
        foreach (var position in eventPositions)
        {
            var distance = position.Index - currentPosition;
            if (distance < 0 && _disableGoBack)
                distance = (allPositions.Length - currentPosition) + position.Index;

            distance = Mathf.Abs(distance);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = position;
            }
        }

        distanceToMove = closestDistance;
        return closestPosition;
    }
}
