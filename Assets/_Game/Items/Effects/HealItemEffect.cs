using UnityEngine;

[CreateAssetMenu(menuName = "Effects/HealItemEffect")]
public class HealItemEffect : ItemEffect
{
    [SerializeField] private bool _percentBased;
    [SerializeField] private int _amount;
    
    public override void ApplyEffect()
    {
        var amount = _amount;
        if (_percentBased)
            amount = Mathf.RoundToInt((amount / 100f) * GameController.Instance.Player.Data.MaxHealth.Value);
        
        GameController.Instance.Player.Data.CurrentHealth.Value += amount;
    }
}