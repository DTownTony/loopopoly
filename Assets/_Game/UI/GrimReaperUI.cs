using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GrimReaperUI : MonoBehaviour
{
    [SerializeField] private EventView _eventView;
    [SerializeField] private Button _tryButton;
    [SerializeField] private Button _guardianAngelButton;
    [SerializeField] private TMP_Text _deathChanceText;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _skipSound;
    
    private GrimReaperUIArgs _currentArgs;

    private void Awake()
    {
        _tryButton.onClick.AddListener(TryButtonPressed);
        _guardianAngelButton.onClick.AddListener(GuardianAngelButtonPressed);
    }

    public void Show(GrimReaperUIArgs args)
    {
        gameObject.SetActive(true);
        _currentArgs = args;
        _guardianAngelButton.interactable = GameController.Instance.Player.Data.HasItem(args.StopEventItem.Key);
        _deathChanceText.SetText("Try My Luck: " + (args.DeathChance * 100).ToString("F2") + "% chance of death");
    }

    private void TryButtonPressed()
    {
        if (Random.value >= _currentArgs.DeathChance)
        {
            GameController.Instance.GameView.EventDetailDisplay.ShowMessage("Lucky!",
                col: new Color32(52, 155, 242, 255));
            _audioSource.PlayOneShot(_skipSound, 1f);
        }
        else
        {
            GameController.Instance.ChangeCurrentState(GameState.Death);
        }

        Hide();
    }

    private void GuardianAngelButtonPressed()
    {
        GameController.Instance.Player.Data.RemoveItem(_currentArgs.StopEventItem.Key);
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        _eventView.Hide();
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }
}

public class GrimReaperUIArgs
{
    public ItemData StopEventItem { get; set; }
    public float DeathChance { get; set; }

    public GrimReaperUIArgs(ItemData stopEventItem, float deathChance)
    {
        StopEventItem = stopEventItem;
        DeathChance = deathChance;
    }
}