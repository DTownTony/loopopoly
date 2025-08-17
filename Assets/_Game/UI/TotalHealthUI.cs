using UnityEngine;
using UnityEngine.UI;

public class TotalHealthUI : MonoBehaviour
{
    [SerializeField] private Image _healthFill;

    private void Start()
    {
        GameController.Instance.Player.Data.CurrentHealth.OnValueChanged += ValueChanged;
        GameController.Instance.Player.Data.MaxHealth.OnValueChanged += MaxHealthUpdated;
    }

    private void MaxHealthUpdated(int maxHealth)
    {
        ValueChanged(GameController.Instance.Player.Data.CurrentHealth.Value);
    }

    private void ValueChanged(int newValue)
    {
        var percent = newValue / (float)GameController.Instance.Player.Data.MaxHealth.Value;
        _healthFill.fillAmount = percent;
    }
}