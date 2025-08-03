using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string Name;
    public bool IsBoss;
    public int Health;
    public int DamageMin;
    public int DamageMax;
    
    public int Experience;
    
    public float HealthBarYOffset;
    
    public GameObject Prefab;
}