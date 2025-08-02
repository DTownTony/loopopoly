using UnityEngine;

[CreateAssetMenu(menuName = "Data/LoopLevelData", fileName = "LoopLevelData")]
public class LoopLevelData : ScriptableObject
{
    public int MaxLoops;
    public EnemyData BossData;
    public EnemyData EnemyData;
}