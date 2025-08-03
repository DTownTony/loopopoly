using System.Collections;
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
    
    public float LoopExponentialValue { get; private set; }= 1.05f;
    public float CombatExponentialValue { get; private set; } = 1.15f;

    [SerializeField] private AudioSource _musicSource;
    
    private void Awake()
    {
        Instance = this;
        LevelData = _loopsLevels[_currentLevelLoop];
        _musicSource.clip = LevelData.Music;
        _musicSource.Play();
        
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
        var boardPosition = _board.GetBoardPositions(Player.CurrentPositionIndex, value);
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
        Player.Data.BossDefeated++;
        
        _loops = 0;
        _currentLevelLoop++;
        if (_currentLevelLoop >= _loopsLevels.Length)
        {
            LoopExponentialValue += .0025f;
            CombatExponentialValue += .0025f;
            _currentLevelLoop = 0;
        }

        LevelData = _loopsLevels[_currentLevelLoop];
        _board.BuildBoard();
        StartCoroutine(DelayNextMusic());
        
        OnLoopsChanged?.Invoke(_loops, LevelData.MaxLoops);
    }

    private IEnumerator DelayNextMusic()
    {
        _musicSource.Stop();
        _musicSource.clip = LevelData.Music;
 
        yield return new WaitForSeconds(3.5f);
        _musicSource.Play();
    }
}

public enum GameState
{
    Bootstrap,
    WaitingForPlayer,
    DiceRolling,
    PlayerMoving,
    PositionEvent,
    Combat,
    Death
}