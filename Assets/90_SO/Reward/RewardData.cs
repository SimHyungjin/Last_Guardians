using UnityEngine;

[CreateAssetMenu(fileName = "NewRewardData", menuName = "Data/Reward Data")]
public class RewardData : ScriptableObject
{
    public int waveID;
    public float equipDropChance;
    public float stoneDropChance;
    public int minGold;
    public int maxGold;
    public int minStone;
    public int maxStone;
    public float gradeMultiplier;
    public int dropGrade;

    public void SetData(int waveID, float equipDropChance, float stoneDropChance,
                        int minGold, int maxGold, int minStone, int maxStone,
                        float gradeMultiplier, int dropGrade)
    {
        this.waveID = waveID;
        this.equipDropChance = equipDropChance;
        this.stoneDropChance = stoneDropChance;
        this.minGold = minGold;
        this.maxGold = maxGold;
        this.minStone = minStone;
        this.maxStone = maxStone;
        this.gradeMultiplier = gradeMultiplier;
        this.dropGrade = dropGrade;
    }
}
