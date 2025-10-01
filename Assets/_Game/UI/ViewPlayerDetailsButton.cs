using UnityEngine;
using UnityEngine.UI;

public class ViewPlayerDetailsButton : MonoBehaviour
{
    [SerializeField] private PlayerDetailsUI _playerDetailsUI;
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(ButtonPressed);
    }

    private void ButtonPressed()
    {
        if (_playerDetailsUI.gameObject.activeSelf)
            _playerDetailsUI.Hide();
        else
            _playerDetailsUI.Show();
    }
}