using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameState CurrentGameState { get; private set; } = GameState.Bootstrap;
    
    [SerializeField] private Player _player;
    [SerializeField] private DiceRoller _diceRoller;
    [SerializeField] private Board _board;

    private void Awake()
    {
        Instance = this;
        ChangeCurrentState(GameState.WaitingForPlayer);
    }

    private void DiceRolled(int value)
    {
        PlayerMove(value);
    }

    public void ChangeCurrentState(GameState newState)
    {
        CurrentGameState = newState;

        switch (newState)
        {
            case GameState.WaitingForPlayer:
                _diceRoller.OnDiceRolled += DiceRolled;
                break;
            case GameState.PlayerMoving:
                _diceRoller.OnDiceRolled -= DiceRolled;
                break;
        }
    }

    private void PlayerMove(int value)
    {
        ChangeCurrentState(GameState.PlayerMoving);
        var boardPosition = _board.GetBoardPositions(_player.CurrentIndex, value);
        var finalPosition = boardPosition[^1];
        _player.Move(boardPosition, () =>
        {
            PositionEventTrigger(finalPosition);
        });
    }

    private void PositionEventTrigger(BoardPosition position)
    {
        ChangeCurrentState(GameState.PositionEvent);
        position.Trigger();
    }
}

public enum GameState
{
    Bootstrap,
    WaitingForPlayer,
    PlayerMoving,
    PositionEvent
}