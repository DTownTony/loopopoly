using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KingsTaxesUI : MonoBehaviour
{
    [SerializeField] private EventView _eventView;
    [SerializeField] private Button _payButton;
    [SerializeField] private Button _skipButton;

    [SerializeField] private TMP_Text _descriptionText;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _goldSound;
    
    private KingsTaxesUIArgs _currentArgs;

    private void Awake()
    {
        _payButton.onClick.AddListener(PayButtonPressed);
        _skipButton.onClick.AddListener(SkipButtonPressed);
    }

    public void Show(KingsTaxesUIArgs args)
    {
        gameObject.SetActive(true);
        _currentArgs = args;
        _skipButton.interactable = GameController.Instance.Player.Data.HasItem(args.StopEventItem.Key);
        
        _descriptionText.SetText($"The king has demanded taxes be paid. You owe {_currentArgs.Amount} gold!");
    }

    private void PayButtonPressed()
    {
        GameController.Instance.Player.Data.Gold.Value -= _currentArgs.Amount;
        _audioSource.PlayOneShot(_goldSound, 1f);
        Hide();
    }

    private void SkipButtonPressed()
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

public class KingsTaxesUIArgs
{
    public ItemData StopEventItem { get; set; }
    public int Amount { get; set; }

    public KingsTaxesUIArgs(ItemData stopEventItem, int amount)
    {
        StopEventItem = stopEventItem;
        Amount = amount;
    }
}