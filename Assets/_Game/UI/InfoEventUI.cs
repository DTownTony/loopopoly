using UnityEngine;
using UnityEngine.UI;

public class InfoEventUI : EventUIBase<EventUIArgsBase>
{
    [SerializeField] private Button _nextButton;

    private void Awake()
    {
        _nextButton.onClick.AddListener(NextButtonPressed);
    }
    
    private void NextButtonPressed()
    {
        Hide();
    }
}
