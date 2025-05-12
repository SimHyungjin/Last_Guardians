using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerUpgrade", menuName = "Data/TowerUpgrade Data")]
[System.Serializable]
public class TowerUpgradeData : ScriptableObject
{
    [Header("�����͸� ����Ʈ")]
    public int totalMasteryPoint;
    public int currentMasteryPoint;

    [Header("���ݸ����͸�")]
    [Header("Ƽ��1")]
    //Ƽ�� 1
    public int attackPowerUpgrade;
    public int attackSpeedUpgrade;
    public int attackRangeUpgrade;
    [Header("Ƽ��2")]
    //Ƽ�� 2
    public int penetrationUpgrade;
    public int continuousAttackUpgrade;
    [Header("Ƽ��3")]
    //Ƽ�� 3
    public int combetMasteryUpgrade;
    public int multipleAttackUpgrade;
    public int bossSlayerUpgrade;

    [Header("ȿ�������͸�")]
    [Header("Ƽ��1")]
    public int effectValueUpgrade;
    public int effectDurationUpgrade;
    public int effectRangeUpgrade;
    [Header("Ƽ��2")]
    public int catalysisUpgrade;
    public int effecttransferUpgrade;

    public void Init(string savePath)
    {
        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, JsonUtility.ToJson(this, true));
            Debug.Log("[TowerUpgradeData] save.json�� ��� ���� ������.");
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
