using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string Name;
    public int Health;
    public int DamageMin;
    public int DamageMax;
    public float Defense;
    
    public int Experience;
    
    public GameObject Prefab;
}