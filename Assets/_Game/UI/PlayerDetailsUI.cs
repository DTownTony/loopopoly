using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailsUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _closeButton;
    
    [SerializeField] private GearSlotUI[] _gearSlots;
    
    [Header("Stats")]
    [SerializeField] private PlayerValueHealthUI _healthStatUI;
    [SerializeField] private PlayerValueUI _damageStatUI;
    [SerializeField] private PlayerValueUI _protectionStatUI;
    [SerializeField] private PlayerValueUI _critChanceStatUI;
    [SerializeField] private PlayerValueUI _critDamageStatUI;
    [SerializeField] private PlayerValueUI _evasionStatUI;
    [SerializeField] private PlayerValueUI _lifeStealStatUI;
    [SerializeField] private PlayerValueUI _luckStatUI;

    private void Start()
    {
        _closeButton.onClick.AddListener(CloseButtonPressed);
    }

    private void CloseButtonPressed()
    {
        Hide();
    }

    public void SetData(PlayerData playerData)
    {
        //stats
        _healthStatUI.SetMaxHealth(playerData.MaxHealth);
        _healthStatUI.SetPlayerValue(playerData.CurrentHealth);
        
        _damageStatUI.SetPlayerValue(playerData.Damage);
        _protectionStatUI.SetPlayerValue(playerData.Protection);
        _critChanceStatUI.SetPlayerValue(playerData.CriticalChance);
        _critDamageStatUI.SetPlayerValue(playerData.CriticalDamage);
        _evasionStatUI.SetPlayerValue(playerData.Evasion);
        _lifeStealStatUI.SetPlayerValue(playerData.LifeSteal);
        _luckStatUI.SetPlayerValue(playerData.Luck);
        
        //gear
        foreach (var gearSlot in _gearSlots)
            gearSlot.Setup(playerData);
    }

    public void Show()
    {
        _canvas.enabled = true;
    }
    
    private void Hide()
    {
        _canvas.enabled = false;
    }
}