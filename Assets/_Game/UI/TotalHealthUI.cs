using UnityEngine;
using UnityEngine.UI;

public class TotalHealthUI : MonoBehaviour
{
    [SerializeField] private Image _healthFill;

    private void Start()
    {
        GameController.Instance.Player.Data.CurrentHealth.OnStatChanged += StatChanged;
        GameController.Instance.Player.Data.OnMaxHealthUpdated += MaxHealthUpdated;
    }

    private void MaxHealthUpdated(int maxHealth)
    {
        StatChanged(GameController.Instance.Player.Data.CurrentHealth.Value);
    }

    private void StatChanged(int newValue)
    {
        var percent = newValue / (float)GameController.Instance.Player.Data.MaxHealth;
        _healthFill.fillAmount = percent;
    }
}