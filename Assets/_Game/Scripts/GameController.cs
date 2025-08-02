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

    public int MaxLoops { get; private set; }
    
    private int _loops;

    public Player Player;
    public EventHandler EventHandler;
    public GameView GameView;

    public LevelData LevelData { get; private set; }
    [SerializeField] private LevelData[] _loopsLevels;
    private int _currentLevelLoop;
    
    [SerializeField] private DiceRoller _diceRoller;
    [SerializeField] private Board _board;

    public const float LOOP_COMBAT_VALUE = 1.25f;
    public const float LOOP_EXPONENTIAL_VALUE = 1.05f;
    
    private void Awake()
    {
        Instance = this;
        LevelData = _loopsLevels[_currentLevelLoop];
        _board.BuildBoard();
        ChangeCurrentState(GameState.WaitingForPlayer);
    }

    private void Start()
    {
        ResetLoops();
        GameView.SetStats(Player.Data);
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
        OnLoopsChanged?.Invoke(_loops, LevelData.MaxLoops);
    }

    public void IncreaseGameLoop()
    {
        _loops++;
        MaxLoops++;
        OnLoopsChanged?.Invoke(_loops, LevelData.MaxLoops);
    }
    
    public bool BossFightAvailable()
    {
        return _loops > LevelData.MaxLoops;
    }

    public void ChangeLevelLoop()
    {
        _loops = 0;
        _currentLevelLoop++;
        if (_currentLevelLoop >= _loopsLevels.Length)
            _currentLevelLoop = 0;

        LevelData = _loopsLevels[_currentLevelLoop];
        _board.BuildBoard();
        
        OnLoopsChanged?.Invoke(_loops, LevelData.MaxLoops);
    }

    public void ReloadGame()
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