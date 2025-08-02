using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollUI : MonoBehaviour
{
    [SerializeField] private DiceRoller _diceRoller;
    [SerializeField] private Button _rollButton;
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _rollText;
    
    private void Awake()
    {
        _rollButton.onClick.AddListener(_diceRoller.RollDice);

        _diceRoller.OnDiceRolled += RefreshText;
        _player.OnMovedSpace += RefreshText;

    }

    private void RefreshText(int value)
    {
        _rollText.SetText($"Roll!\n({value})");
    }
}
