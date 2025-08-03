using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollUI : MonoBehaviour
{
    [SerializeField] private DiceRoller _diceRoller;
    [SerializeField] private Button _rollButton;
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _rollText;
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _rollSound;
    
    private void Awake()
    {
        _rollButton.onClick.AddListener(RollDiceButtonPressed);

        _diceRoller.OnDiceRolled += RefreshText;
        _player.OnMovedSpace += RefreshText;
    }

    private void RollDiceButtonPressed()
    {
        StartCoroutine(DelayRoll());
    }

    private IEnumerator DelayRoll()
    {
        _audioSource.PlayOneShot(_rollSound[Random.Range(0, _rollSound.Length)], .35f);
        yield return new WaitForSeconds(.1f);
        _diceRoller.RollDice();
    }

    private void RefreshText(int value)
    {
        _rollText.SetText($"Roll!\n({value})");
    }
}
