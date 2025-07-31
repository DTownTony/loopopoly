using UnityEngine;
using Random = UnityEngine.Random;

public class DiceRoller : MonoBehaviour
{
    public delegate void DiceRolledDelegate(int value);
    public event DiceRolledDelegate OnDiceRolled;
    
    [SerializeField] private Dice _dice;

    private bool _isRolling;

    private void Update()
    {
        if (Mathf.Approximately(_dice.Rigidbody.linearVelocity.magnitude, 0f) && _isRolling)
            CompleteRoll();
    }

    private void ApplyPhysics()
    {
        var x = Random.Range(0, 360);
        var y = Random.Range(0, 360);
        var z = Random.Range(0, 360);
        var rotation = Quaternion.Euler(x, y, z);
        
        x = Random.Range(0, 25);
        y = Random.Range(10, 25);
        z = Random.Range(0, 25);
        var force = new Vector3(x, y, z);

        x = Random.Range(0, 50);
        y = Random.Range(0, 50);
        z = Random.Range(0, 50);
        var torque = new Vector3(x, y, z);
        
        _dice.transform.rotation = rotation;
        _dice.Rigidbody.linearVelocity = force;

        _dice.Rigidbody.maxAngularVelocity = 1000;
        _dice.Rigidbody.AddTorque(torque, ForceMode.VelocityChange);

        _isRolling = true;
    }

    private void CompleteRoll()
    {
        _isRolling = false;
        var value = _dice.GetFaceValue();
        Debug.Log("Dice rolled: " + value);
        OnDiceRolled?.Invoke(value);
    }

    public void RollDice()
    {
        //todo: record and bake animation
        ApplyPhysics();
    }
}