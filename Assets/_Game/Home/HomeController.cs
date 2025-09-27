using UnityEngine;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    [Header("Gear")]
    [SerializeField] private Button _gearButton;
    [SerializeField] private PlayerGearUI _playerGearUI;

    private void Start()
    {
        _gearButton.onClick.AddListener(_playerGearUI.Toggle);
    }
}
