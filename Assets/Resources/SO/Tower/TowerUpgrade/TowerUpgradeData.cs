using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SerializableTowerUpgradeData
{
    public int totalMasteryPoint;
    public int currentMasteryPoint;
    public int usedMasteryPoint;
    public List<int> currentLevel;
    // 생성자
    public SerializableTowerUpgradeData(TowerUpgradeData towerUpgradeData)
    {
        totalMasteryPoint = towerUpgradeData.totalMasteryPoint;
        currentMasteryPoint = towerUpgradeData.currentMasteryPoint;
        usedMasteryPoint = towerUpgradeData.usedMasteryPoint;
        currentLevel = new List<int>(towerUpgradeData.currentLevel);
    }
}

public enum TowerUpgradeType
{
    AttackPower,
    AttackSpeed,
    AttackRange,
    Penetration,
    ContinuousAttack,
    CombetMastery,
    MultipleAttack,
    BossSlayer,
    EffectValue,
    EffectDuration,
    EffectRange,
    Catalysis,
    EffectTransfer
}

[CreateAssetMenu(fileName = "TowerUpgrade", menuName = "Data/TowerUpgrade Data")]
public class TowerUpgradeData : ScriptableObject
{
    [Header("마스터리 포인트")]
    public int totalMasteryPoint;
    public int currentMasteryPoint;
    public int usedMasteryPoint;
    public List<int> currentLevel = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int maxLevel = 3;
    [Header("설명스크립트")]
    public string[] description;

    public void Init()
    {

        string savePath = Application.persistentDataPath + "/save.json";
        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, JsonUtility.ToJson(this, true));
            Debug.Log("[TowerUpgradeData] save.json이 없어서 새로 생성함.");
        }
        else
        {
            string json = File.ReadAllText(savePath);
            var save = JsonUtility.FromJson<SaveData>(json);
            if (save.towerUpgradedata.currentLevel.Count < currentLevel.Count || save.towerUpgradedata.currentLevel==null)
            {
                SerializableTowerUpgradeData towerUpgradeData = new SerializableTowerUpgradeData(this);
                save.towerUpgradedata = towerUpgradeData;
                string newJson = JsonUtility.ToJson(save, true);
                File.WriteAllText(savePath, newJson);
            }
            else
            {
                totalMasteryPoint = save.towerUpgradedata.totalMasteryPoint;
                currentMasteryPoint = save.towerUpgradedata.currentMasteryPoint;
                usedMasteryPoint = save.towerUpgradedata.usedMasteryPoint;
                currentLevel = new List<int>(save.towerUpgradedata.currentLevel);
            }
        }
        usedMasteryPoint = totalMasteryPoint - currentMasteryPoint;
    }
}

//[Header("공격마스터리")]
//[Header("티어1")]
//public int attackPowerUpgrade;
//public int attackSpeedUpgrade;
//public int attackRangeUpgrade;
//[Header("티어2")]
//public int penetrationUpgrade;
//public int continuousAttackUpgrade;
//[Header("티어3")]
//public int combetMasteryUpgrade;
//public int multipleAttackUpgrade;
//public int bossSlayerUpgrade;

//[Header("효과마스터리")]
//[Header("티어1")]
//public int effectValueUpgrade;
//public int effectDurationUpgrade;
//public int effectRangeUpgrade;
//[Header("티어2")]
//public int catalysisUpgrade;
//public int effecttransferUpgrade;
