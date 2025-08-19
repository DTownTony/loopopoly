using UnityEngine;

[CreateAssetMenu(menuName = "Data/LoopLevelData", fileName = "LoopLevelData")]
public class LevelData : ScriptableObject
{
    public int MaxLoops;
    public EnemyData BossData;
    public AudioClip Music;

    [SerializeField] private Board[] _boards;
    [SerializeField] private EnemyData[] _enemyData;

    public EnemyData GetEnemyDifficulty(CombatDifficulty difficulty)
    {
        return _enemyData[(int)difficulty];
    }

    public Board GetBoard(int index = -1)
    {
        return index > 0 ? _boards[index] : _boards[Random.Range(0, _boards.Length)];
    }
}