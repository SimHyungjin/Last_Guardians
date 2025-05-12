using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgrade", menuName = "Data/TowerUpgrade Data")]
[System.Serializable]
public class TowerUpgradeData : ScriptableObject
{
    [Header("마스터리 포인트")]
    public int totalMasteryPoint;
    public int currentMasteryPoint;

    [Header("공격마스터리")]
    [Header("티어1")]
    //티어 1
    public int attackPowerUpgrade;
    public int attackSpeedUpgrade;
    public int attackRangeUpgrade;
    [Header("티어2")]
    //티어 2
    public int penetrationUpgrade;
    public int continuousAttackUpgrade;
    [Header("티어3")]
    //티어 3
    public int combetMasteryUpgrade;
    public int multipleAttackUpgrade;
    public int bossSlayerUpgrade;

    [Header("효과마스터리")]
    [Header("티어1")]
    public int effectValueUpgrade;
    public int effectDurationUpgrade;
    public int effectRangeUpgrade;
    [Header("티어2")]
    public int catalysisUpgrade;
    public int effecttransferUpgrade;

    public void Init(string savePath)
    {
        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, JsonUtility.ToJson(this, true));
            Debug.Log("[TowerUpgradeData] save.json이 없어서 새로 생성함.");
        }
        else
        {
            string json = File.ReadAllText(savePath);
            TowerUpgradeData data = JsonUtility.FromJson<TowerUpgradeData>(json);
            attackPowerUpgrade = data.attackPowerUpgrade;
            attackSpeedUpgrade = data.attackSpeedUpgrade;
            attackRangeUpgrade = data.attackRangeUpgrade;
            penetrationUpgrade = data.penetrationUpgrade;
            continuousAttackUpgrade = data.continuousAttackUpgrade;
            combetMasteryUpgrade = data.combetMasteryUpgrade;
            multipleAttackUpgrade = data.multipleAttackUpgrade;
            bossSlayerUpgrade = data.bossSlayerUpgrade;
            effectValueUpgrade = data.effectValueUpgrade;
            effectDurationUpgrade = data.effectDurationUpgrade;
            effectRangeUpgrade = data.effectRangeUpgrade;
            catalysisUpgrade = data.catalysisUpgrade;
            effecttransferUpgrade = data.effecttransferUpgrade;
        }
    }

}
