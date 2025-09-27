using System.Collections.Generic;
using UnityEngine;

public class GearInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private GearUI _gearUIPrefab;

    [SerializeField] private GearData _fakeGearData;
    
    public void Setup()
    {
        //fake gear setup
        var testGear = new GearInventory()
        {
            Inventory = new List<Gear>()
        };

        for (var i = 0; i < 30; i++)
            testGear.Inventory.Add(new Gear(_fakeGearData));


        foreach (var gear in testGear.Inventory)
        {
            var gearUI = Instantiate(_gearUIPrefab, _contentContainer);
            gearUI.Setup(gear);
        }
    }
}