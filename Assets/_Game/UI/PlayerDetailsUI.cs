using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailsUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _closeButton;

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
        _canvas.enabled = true;
    }
    
    private void Hide()
    {
        _canvas.enabled = false;
    }
}