using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailsUI : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private GearSlotUI[] _gearSlots;

    private void Start()
    {
        _closeButton.onClick.AddListener(CloseButtonPressed);
    }

    private void CloseButtonPressed()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}