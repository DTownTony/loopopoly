using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceEventUI : EventUIBase<ChoiceEventUIArgs>
{ 
    [SerializeField] private Button _positiveButton;
    [SerializeField] private Button _negativeButton;
    
    [SerializeField] private TMP_Text _positiveText;
    [SerializeField] private TMP_Text _negativeText;
    
    private void Awake()
    {
        _positiveButton.onClick.AddListener(PositiveButtonPressed);
        _negativeButton.onClick.AddListener(NegativeButtonPressed);
    }

    protected override void Show(ChoiceEventUIArgs args)
    {
        base.Show(args);
        
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
}

public class ChoiceEventUIArgs : EventUIArgsBase
{
    public readonly string PositiveButtonString;
    public readonly string NegativeButtonString;
    
    public readonly Action PositiveButtonEvent;
    public readonly Action NegativeButtonEvent;

    public readonly bool PositiveButtonEnabled;

    public ChoiceEventUIArgs(string header, Sprite icon, string description, 
        Action positiveButtonEvent, Action negativeButtonEvent, string positiveButtonString, string negativeButtonString,
        bool positiveButtonEnabled = true) : base(header, icon, description)
    {
        PositiveButtonEvent = positiveButtonEvent;
        NegativeButtonEvent = negativeButtonEvent;
        PositiveButtonString = positiveButtonString;
        NegativeButtonString = negativeButtonString;
        
        PositiveButtonEnabled = positiveButtonEnabled;
    }
}