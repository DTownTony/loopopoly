using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathView : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private TMP_Text _statsText;

    private void Awake()
    {
        _menuButton.onClick.AddListener(MenuButtonPressed);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _statsText.SetText($"Total Loops {GameController.Instance.MaxLoops}\n" +
                           $"Bosses Defeated: {GameController.Instance.Player.Data.BossDefeated}");
    }

    private void MenuButtonPressed()
    {
        SceneManager.LoadScene("Menu");
    }
}