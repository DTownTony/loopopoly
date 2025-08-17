using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private TMP_Text _statAmountText;
    
    [SerializeField] private StatUpgradeButton _maxHealthButton;
    [SerializeField] private StatUpgradeButton _damageButton;
    [SerializeField] private StatUpgradeButton _criticalChanceButton;
    [SerializeField] private StatUpgradeButton _criticalDamageButton;
    [SerializeField] private StatUpgradeButton _evasionButton;
    
    [SerializeField] private Button _closeButton;

    private void Start()
    {
        _closeButton.onClick.AddListener(Hide);
        
        _maxHealthButton.SetStat(GameController.Instance.Player.Data.MaxHealth);
        _damageButton.SetStat(GameController.Instance.Player.Data.Damage);
        _criticalChanceButton.SetStat(GameController.Instance.Player.Data.CriticalChance);
        _criticalDamageButton.SetStat(GameController.Instance.Player.Data.CriticalDamage);
        _evasionButton.SetStat(GameController.Instance.Player.Data.Evasion);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, .35f).SetDelay(.5f);

        _closeButton.interactable = false;

        StatPointChanged(GameController.Instance.Player.Data.StatPoints.Value);
        GameController.Instance.Player.Data.StatPoints.OnValueChanged += StatPointChanged;
    }

    private void StatPointChanged(int newValue)
    {
        _statAmountText.SetText($"Stat Points: <color=orange>{newValue}</color>");
        _closeButton.interactable = newValue <= 0;
    }

    private void Hide()
    {
        _maxHealthButton.Confirm();
        _damageButton.Confirm();
        _criticalChanceButton.Confirm();
        _criticalDamageButton.Confirm();
        _evasionButton.Confirm();
        
        GameController.Instance.Player.Data.StatPoints.OnValueChanged -= StatPointChanged;
        gameObject.SetActive(false);
    }
}