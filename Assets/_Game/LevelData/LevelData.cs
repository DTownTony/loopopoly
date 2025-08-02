using UnityEngine;

[CreateAssetMenu(menuName = "Data/LoopLevelData", fileName = "LoopLevelData")]
public class LevelData : ScriptableObject
{
    public int MaxLoops;
    public EnemyData BossData;
    public EnemyData EnemyData;
    public LevelPieces LevelPieces;
}