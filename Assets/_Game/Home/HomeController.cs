using UnityEngine;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    private void Start()
    {
        _startButton.onClick.AddListener(StartButtonPressed);
    }

    private void StartButtonPressed()
    {
        GlobalManagers.Instance.GameSetup.LoadGameScene();
    }
}