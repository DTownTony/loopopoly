using TMPro;
using UnityEngine;

public class PlayerValueUI : MonoBehaviour
{
    [SerializeField] protected TMP_Text _amountText;

    public void SetPlayerValue(PlayerValue playerValue)
    {
        playerValue.OnValueChanged += Set;
        Set(playerValue.Value);
    }

    protected virtual void Set(int amount)
    {
        _amountText.SetText(amount.ToString());
    }
}