using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _discordButton;

    private void Awake()
    {
        _playButton.onClick.AddListener(PlayButtonPressed);
        _discordButton.onClick.AddListener(DiscordButtonPressed);
    }

    private void PlayButtonPressed()
    {
        SceneManager.LoadScene("Game");
    }
    
    private void DiscordButtonPressed()
    {
        Application.OpenURL("https://discord.com/invite/gnuAtqNyt9");
    }
}