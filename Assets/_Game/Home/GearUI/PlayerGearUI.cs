using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGearUI : MonoBehaviour
{
    [SerializeField] private GearInventoryUI _gearInventoryUI;

    private void Start()
    {
        //todo: equipped ui
        _gearInventoryUI.Setup();
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}