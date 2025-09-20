using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceEventUI : MonoBehaviour
{ 
    [SerializeField] private EventView _eventView;

    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _descriptionText;
    
    [SerializeField] private Button _positiveButton;
    [SerializeField] private Button _negativeButton;
    
    [SerializeField] private TMP_Text _positiveText;
    [SerializeField] private TMP_Text _negativeText;
    
    private ChoiceEventUIArgs _currentArgs;

    private void Awake()
    {
        _positiveButton.onClick.AddListener(PositiveButtonPressed);
        _negativeButton.onClick.AddListener(NegativeButtonPressed);
    }

    public void Show(ChoiceEventUIArgs args)
    {
        gameObject.SetActive(true);
        _currentArgs = args;
        
        _headerText.SetText(_currentArgs.Header);
        _icon.sprite = _currentArgs.Icon;
        _descriptionText.SetText(_currentArgs.Description);
        
        _positiveText.SetText(_currentArgs.PositiveButtonString);
        _negativeText.SetText(_currentArgs.NegativeButtonString);
        
        _positiveButton.interactable = _currentArgs.PositiveButtonEnabled;
    }
    
    private void PositiveButtonPressed()
    {
        _currentArgs.PositiveButtonEvent?.Invoke();
        Hide();
    }

    private void NegativeButtonPressed()
    {
        _currentArgs.NegativeButtonEvent?.Invoke();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        _eventView.Hide();
        GameController.Instance.ChangeCurrentState(GameState.WaitingForPlayer);
    }
}

public class ChoiceEventUIArgs
{
    public string Header;
    public Sprite Icon;
    public string Description;
    
    public string PositiveButtonString;
    public string NegativeButtonString;
    
    public Action PositiveButtonEvent;
    public Action NegativeButtonEvent;

    public bool PositiveButtonEnabled;

    public ChoiceEventUIArgs(string header, Sprite icon, string description, 
        Action positiveButtonEvent, Action negativeButtonEvent, string positiveButtonString, string negativeButtonString,
        bool positiveButtonEnabled = true)
    {
        Header = header;
        Icon = icon;
        Description = description;
        PositiveButtonEvent = positiveButtonEvent;
        NegativeButtonEvent = negativeButtonEvent;
        PositiveButtonString = positiveButtonString;
        NegativeButtonString = negativeButtonString;
        
        PositiveButtonEnabled = positiveButtonEnabled;
    }
}