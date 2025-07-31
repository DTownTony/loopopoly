using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public delegate void GameStateChangedDelegate(GameState newState);
    public event GameStateChangedDelegate OnGameStateChanged;
    
    public delegate void OnLoopsChangedDelegate(int loops, int maxLoops);
    public event OnLoopsChangedDelegate OnLoopsChanged;
    
    public GameState CurrentGameState { get; private set; } = GameState.Bootstrap;

    private int _loops;
    private int _maxLoops;

    public Player Player;
    public EventHandler EventHandler;

    public LoopLevelData CurrentLoopLevelData => _loopLevelData;
    [SerializeField] private LoopLevelData _loopLevelData;
    
    [SerializeField] private DiceRoller _diceRoller;
    [SerializeField] private Board _board;
    
    [SerializeField] private GameView _gameView;

    private void Awake()
    {
        Instance = this;
        ChangeCurrentState(GameState.WaitingForPlayer);
    }

    private void Start()
    {
        //todo: setup random loop
        ResetLoops();
        _gameView.SetStats(Player.Data);
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
        
        OnGameStateChanged?.Invoke(CurrentGameState);
    }

    private void PlayerMove(int value)
    {
        ChangeCurrentState(GameState.PlayerMoving);
        var boardPosition = _board.GetBoardPositions(Player.CurrentIndex, value);
        var finalPosition = boardPosition[^1];
        Player.Move(boardPosition, () =>
        {
            PositionEventTrigger(finalPosition);
        });
    }

    private void PositionEventTrigger(BoardPosition position)
    {
        ChangeCurrentState(GameState.PositionEvent);
        position.Trigger();
    }

    private void ResetLoops()
    {
        _loops = 0;
        OnLoopsChanged?.Invoke(_loops, _loopLevelData.MaxLoops);
    }

    public void IncreaseGameLoop()
    {
        _loops++;
        _maxLoops++;
        OnLoopsChanged?.Invoke(_loops, _loopLevelData.MaxLoops);
    }
    
    public bool BossFightAvailable()
    {
        return _loops > _loopLevelData.MaxLoops;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Game");
    }
}

public enum GameState
{
    Bootstrap,
    WaitingForPlayer,
    DiceRolling,
    PlayerMoving,
    PositionEvent,
    Combat
}