using UnityEngine;

[CreateAssetMenu(menuName = "Data/LoopLevelData", fileName = "LoopLevelData")]
public class LevelData : ScriptableObject
{
    public int MaxLoops;
    public EnemyData BossData;
    public AudioClip Music;

    [SerializeField] private EnemyData[] _enemyData;

    public EnemyData GetEnemyDifficulty(CombatDifficulty difficulty)
    {
        return _enemyData[(int)difficulty];
    }
}