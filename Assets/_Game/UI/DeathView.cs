using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private TMP_Text _statsText;

    [Header("Audio")] 
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _loseSound;
    
    private void Awake()
    {
        _continueButton.onClick.AddListener(ContinueButtonPressed);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _statsText.SetText($"Total Loops {GameController.Instance.TotalLoopCount}\n" +
                           $"Bosses Defeated: {GameController.Instance.Player.Data.BossDefeated}\n" +
                           $"Total Dice Rolls: {GameController.Instance.Player.Data.TotalMoves}");
        
        _musicSource.Stop();
        _audioSource.PlayOneShot(_loseSound, .5f);
    }

    private void ContinueButtonPressed()
    {
        SceneManager.LoadScene("Home");
    }
}