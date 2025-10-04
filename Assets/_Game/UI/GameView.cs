using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public EventDetailDisplay EventDetailDisplay;
    
    [SerializeField] private PlayerDetailsUI _playerDetailsUI;
    [SerializeField] private PlayerValueUI _goldUI;
    [SerializeField] private PlayerValueUI _healthUI;
    [SerializeField] private PlayerValueUI _damageUI;
    [SerializeField] private PlayerValueUI _defenseUI;
    [SerializeField] private PlayerValueUI _levelUI;
    [SerializeField] private PlayerValueUI _experienceUI;
    [SerializeField] private DeathView _deathView;
    [SerializeField] private LevelUpView _levelUpView;

    [SerializeField] private TMP_Text _bossLoopsText;

    [SerializeField] private Button _rollButton;

    private void Start()
    {
        GameController.Instance.OnGameStateChanged += GameStateChanged;
        GameController.Instance.OnLoopsChanged += LoopsChanged;
        GameController.Instance.Player.Data.Level.OnValueChanged += LevelUp;
        
        LoopsChanged(0, GameController.Instance.MaxLoops);
    }
    
    private void GameStateChanged(GameState newState)
    {
        _rollButton.interactable = newState == GameState.WaitingForPlayer;

        if (newState == GameState.Death)
            _deathView.Show();
    }
    
    private void LoopsChanged(int loops, int maxLoops)
    {
        var loopsLeft = maxLoops - loops;
        _bossLoopsText.SetText(loopsLeft > 0 ? "Loops before boss: " + loopsLeft : "Boss Incoming!");

        if (loopsLeft < 0)
            _bossLoopsText.SetText("BOSS FIGHT!");
    }

    public void SetData(PlayerData data)
    {
        _goldUI.SetPlayerValue(data.Gold);
        _healthUI.SetPlayerValue(data.CurrentHealth);
        _damageUI.SetPlayerValue(data.Damage);
        _defenseUI.SetPlayerValue(data.Protection);
        _levelUI.SetPlayerValue(data.Level);
        _experienceUI.SetPlayerValue(data.Experience);
        
        _playerDetailsUI.SetData(data);
    }

    private void LevelUp(int newLevel)
    {
        _levelUpView.Show();
    }
}
