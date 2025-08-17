using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgradeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private Button _increaseButton;
    [SerializeField] private Button _decreaseButton;
    
    private PlayerValue _playerValue;
    private int _increaseAmount;

    private void Start()
    {
        _increaseButton.onClick.AddListener(IncreaseAmount);
        _decreaseButton.onClick.AddListener(DecreaseAmount);
        
        _decreaseButton.interactable = false;
        
        GameController.Instance.Player.Data.StatPoints.OnValueChanged += StatPointChanged;
    }

    public void SetStat(PlayerValue value)
    {
        _playerValue = value;
        _playerValue.OnValueChanged += StatValueChanged;
        
        _valueText.SetText(_playerValue.Value.ToString());
    }

    private void IncreaseAmount()
    {
        _increaseAmount++;
        GameController.Instance.Player.Data.StatPoints.Value--;
        
        RefreshDisplay();
    }

    private void DecreaseAmount()
    {
        _increaseAmount--;
        GameController.Instance.Player.Data.StatPoints.Value++;
        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        _valueText.SetText((_playerValue.Value + _increaseAmount).ToString());
        _decreaseButton.interactable = _increaseAmount > 0;
        _increaseButton.interactable = GameController.Instance.Player.Data.StatPoints.Value > 0;
    }

    private void StatPointChanged(int newValue)
    {
        RefreshDisplay();
    }

    private void StatValueChanged(int newValue)
    {
        _valueText.SetText(newValue.ToString());
        RefreshDisplay();
    }

    public void Confirm()
    {
        _decreaseButton.interactable = false;
        
        if (_increaseAmount <= 0)
            return;
        
        _playerValue.Value += _increaseAmount;
        _increaseAmount = 0;
    }
}