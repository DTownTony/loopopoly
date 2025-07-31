using UnityEngine;

public class Dice : MonoBehaviour
{
    public Rigidbody Rigidbody;
    
    [SerializeField] private Transform _diceTransform;
    
    [SerializeField] private Transform[] _sideTransforms;

    public int GetFaceValue()
    {
        var highestY = float.MinValue;
        var  highestIndex = -1;
        for (var i = 0; i < _sideTransforms.Length; i++)
        {
            var side = _sideTransforms[i];

            if (side.position.y <= highestY)
                continue;
            
            highestY = side.position.y;
            highestIndex = i;
        }

        return highestIndex + 1;
    }
    
    public void RotateFace(Vector2 rotation)
    {
        _diceTransform.Rotate(rotation);
    }
}