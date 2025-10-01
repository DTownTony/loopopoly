using UnityEngine;
using UnityEngine.UI;

public class GearSlotUI : MonoBehaviour
{
    [SerializeField] private Image _bg;
    [SerializeField] private Image _icon;

    public void SetGear(Gear gear)
    {
        _icon.color = Color.white;
        _icon.sprite = gear.Data.Icon;
    }
}