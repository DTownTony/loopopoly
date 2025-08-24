using UnityEngine;

[CreateAssetMenu(menuName = "Effects/MoveItemEffect")]
public class MoveItemEffect : ItemEffect
{
    [SerializeField] private int _amount;
    
    public override void ApplyEffect()
    {
        GameController.Instance.PlayerMove(_amount);
    }
}
