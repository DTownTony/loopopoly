using UnityEngine;
using UnityEngine.UI;

public class GearSlotUI : MonoBehaviour
{
    [SerializeField] private Image _bg;
    [SerializeField] private Image _icon;
    
    [SerializeField] private GearType _gearType;

    private Gear _gear;

    public void Setup(PlayerData playerData)
    {
        playerData.OnGearAdded += SetGear;
        playerData.OnGearRemoved += RemoveGear;
    }

    private void SetGear(Gear gear)
    {
        if (gear.Data.GearType != _gearType)
            return;
        
        _gear = gear;
        _icon.color = Color.white;
        _icon.sprite = _gear.Data.Icon;
    }

    private void RemoveGear(Gear gear)
    {
        if (gear.Data.GearType != _gearType)
            return;

        _gear = null;
    }
}