using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/MoveToItemEffect")]
public class MoveToItemEffect : ItemEffect
{
    [SerializeField] private BoardEvent _eventToMoveTo;
    [SerializeField] private bool _disableGoBack;
    
    public override void ApplyEffect()
    {
        var currentPosition = GameController.Instance.Player.CurrentPositionIndex; 
        GetBoardPosition(currentPosition, out var distance);
        
        GameController.Instance.PlayerMove(distance);
    }

    private void GetBoardPosition(int currentPosition, out int distanceToMove)
    {
        var eventPositions = new List<BoardPosition>();

        var allPositions = GameController.Instance.Board.GetBoardPositions(); //reverse so forward is prioritized 
        foreach (var position in allPositions)
        {
            if (position.IsSameEvent(_eventToMoveTo))
                eventPositions.Add(position);
        }
        
        //find nearest position
        var closestDistancePositive = int.MaxValue;
        var closestDistanceNegative = -int.MaxValue;
        foreach (var position in eventPositions)
        {
            var distance = position.Index - currentPosition;
            //loop around to start
            if (distance < 0 && _disableGoBack)
                distance = (allPositions.Length - currentPosition) + position.Index;
            
            var isPositive = distance >= 0;
            if (isPositive)
            {
                if (distance < closestDistancePositive)
                    closestDistancePositive = distance;
            }
            else
            {
                if (distance > closestDistanceNegative)
                    closestDistanceNegative = distance;
            }
        }
        
        distanceToMove = closestDistanceNegative > -int.MaxValue && Mathf.Abs(closestDistanceNegative) < closestDistancePositive 
            ? closestDistanceNegative 
            : closestDistancePositive;
    }
}
