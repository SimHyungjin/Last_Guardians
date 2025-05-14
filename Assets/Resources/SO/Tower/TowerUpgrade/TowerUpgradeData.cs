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
    // ������
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
    [Header("�����͸� ����Ʈ")]
    public int totalMasteryPoint;
    public int currentMasteryPoint;
    public int usedMasteryPoint;
    public List<int> currentLevel = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int maxLevel = 3;
    [Header("����ũ��Ʈ")]
    public string[] description;

    public void Init()
    {

        string savePath = Application.persistentDataPath + "/save.json";
        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, JsonUtility.ToJson(this, true));
            Debug.Log("[TowerUpgradeData] save.json�� ��� ���� ������.");
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

//[Header("���ݸ����͸�")]
//[Header("Ƽ��1")]
//public int attackPowerUpgrade;
//public int attackSpeedUpgrade;
//public int attackRangeUpgrade;
//[Header("Ƽ��2")]
//public int penetrationUpgrade;
//public int continuousAttackUpgrade;
//[Header("Ƽ��3")]
//public int combetMasteryUpgrade;
//public int multipleAttackUpgrade;
//public int bossSlayerUpgrade;

//[Header("ȿ�������͸�")]
//[Header("Ƽ��1")]
//public int effectValueUpgrade;
//public int effectDurationUpgrade;
//public int effectRangeUpgrade;
//[Header("Ƽ��2")]
//public int catalysisUpgrade;
//public int effecttransferUpgrade;
