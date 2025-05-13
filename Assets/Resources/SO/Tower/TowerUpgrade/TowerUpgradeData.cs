using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TowerUpgradeInfo
{
    public int currentLevel;
    public int maxLevel = 3;
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

[CreateAssetMenu(fileName = "towerUpgrade", menuName = "Data/towerUpgrade Data")]
[System.Serializable]
public class TowerUpgradeData : ScriptableObject
{
    [Header("마스터리 포인트")]
    public int totalMasteryPoint;
    public int currentMasteryPoint;
    public int usedMasteryPoint;
    public string[] description;
    public Dictionary<TowerUpgradeType, TowerUpgradeInfo> upgradeData;

    public void Init()
    {
        usedMasteryPoint = totalMasteryPoint - currentMasteryPoint;
        upgradeData = new Dictionary<TowerUpgradeType, TowerUpgradeInfo>
        {
            { TowerUpgradeType.AttackPower, new TowerUpgradeInfo() },
            { TowerUpgradeType.AttackSpeed, new TowerUpgradeInfo() },
            { TowerUpgradeType.AttackRange, new TowerUpgradeInfo() },
            { TowerUpgradeType.Penetration, new TowerUpgradeInfo() },
            { TowerUpgradeType.ContinuousAttack, new TowerUpgradeInfo() },
            { TowerUpgradeType.CombetMastery, new TowerUpgradeInfo() },
            { TowerUpgradeType.MultipleAttack, new TowerUpgradeInfo() },
            { TowerUpgradeType.BossSlayer, new TowerUpgradeInfo() },
            { TowerUpgradeType.EffectValue, new TowerUpgradeInfo() },
            { TowerUpgradeType.EffectDuration, new TowerUpgradeInfo() },
            { TowerUpgradeType.EffectRange, new TowerUpgradeInfo() },
            { TowerUpgradeType.Catalysis, new TowerUpgradeInfo() },
            { TowerUpgradeType.EffectTransfer, new TowerUpgradeInfo() }
        };

        string savePath = Application.persistentDataPath + "/save.json";
        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, JsonUtility.ToJson(this, true));
            Debug.Log("[TowerUpgradeData] save.json이 없어서 새로 생성함.");
        }
        else
        {
            //string json = File.ReadAllText(savePath);
            //TowerUpgradeData data = JsonUtility.FromJson<TowerUpgradeData>(json);
            //foreach (var kvp in data.upgradeData)
            //{
            //    upgradeData[kvp.Key] = kvp.Value;
            //}
        }
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
