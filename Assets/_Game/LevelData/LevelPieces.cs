using UnityEngine;

[CreateAssetMenu(fileName = "Data/LevelPieces", menuName = "LevelPieces")]
public class LevelPieces : ScriptableObject
{
    public string Name;
    public GameObject[] CornerPieces;
    public GameObject[] BoardPieces;
    public GameObject BattlePiece;
    public GameObject BossBattlePiece;
    public GameObject ShopPiece;
    public GameObject StatsIncrease;
    public GameObject Coins;
    public GameObject Death;
}
