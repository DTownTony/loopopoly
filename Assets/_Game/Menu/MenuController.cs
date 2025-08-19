using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _discordButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private SaveSelectorView _saveSelectorView;

    private void Awake()
    {
        _playButton.onClick.AddListener(PlayButtonPressed);
        _discordButton.onClick.AddListener(DiscordButtonPressed);
        _settingsButton.onClick.AddListener(SettingsButtonPressed);
        _quitButton.onClick.AddListener(QuitButtonPressed);
    }

    private void PlayButtonPressed()
    {
        _saveSelectorView.Show();
    }
    
    private void DiscordButtonPressed()
    {
        Application.OpenURL("https://discord.com/invite/gnuAtqNyt9");
    }

    private void SettingsButtonPressed()
    {
        Debug.Log("Implement me");
    }
    
    private void QuitButtonPressed()
    {
        Application.Quit();
    }
}