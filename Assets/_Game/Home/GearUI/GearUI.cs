using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearUI : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _levelText;

    [SerializeField] private Button _button;

    private Gear _gear;

    private void Start()
    {
        _button.onClick.AddListener(ButtonPressed);
    }

    public void Setup(Gear gear)
    {
        _gear = gear;
        _icon.sprite = _gear.Data.Icon;
        _levelText.SetText($"Lv. {gear.Level + 1}");
    }
    
    private void ButtonPressed()
    {
        Debug.Log("gear: " + _gear.Data.Name);
    }
}