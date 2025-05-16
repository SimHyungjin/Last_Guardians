using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SerializableTowerUpgradeData
{
    public int totalMasteryPoint;
    public int currentMasteryPoint;
    public int usedMasteryPoint;

    public int towerPoint;

    public List<int> currentLevel;
    // ������
    public SerializableTowerUpgradeData(TowerUpgradeData towerUpgradeData)
    {
        totalMasteryPoint = towerUpgradeData.totalMasteryPoint;
        currentMasteryPoint = towerUpgradeData.currentMasteryPoint;
        usedMasteryPoint = towerUpgradeData.usedMasteryPoint;
        towerPoint = towerUpgradeData.towerPoint;
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
    EffectTransfer,
    Emergencyresponse
}

[CreateAssetMenu(fileName = "TowerUpgrade", menuName = "Data/TowerUpgrade Data")]
public class TowerUpgradeData : ScriptableObject
{
    [Header("�����͸� ����Ʈ")]
    public int totalMasteryPoint;
    public int currentMasteryPoint;
    public int usedMasteryPoint;

    public int towerPoint;

    public List<int> currentLevel;
    public int maxLevel = 3;
    [Header("����ũ��Ʈ")]
    public string[] description;

    public void Init()
    {
        totalMasteryPoint = 0;
        currentMasteryPoint = 0;
        usedMasteryPoint = 0;
        towerPoint = 0;
        currentLevel = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        string savePath = Application.persistentDataPath + "/save.json";
        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, JsonUtility.ToJson(this, true));
            Debug.Log("[TowerUpgradeData] save.json�� ��� ���� ������.");
        }
        else
        {
            string json = File.ReadAllText(savePath);
            SaveData save = JsonUtility.FromJson<SaveData>(json);
            if (save == null || save.towerUpgradedata.currentLevel == null || save.towerUpgradedata.currentLevel.Count < currentLevel.Count)
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
                towerPoint = save.towerUpgradedata.towerPoint;
                currentLevel = new List<int>(save.towerUpgradedata.currentLevel);
            }
        }
        usedMasteryPoint = totalMasteryPoint - currentMasteryPoint;
    }
    public void Save()
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
            SaveData save = JsonUtility.FromJson<SaveData>(json);
            SerializableTowerUpgradeData towerUpgradeData = new SerializableTowerUpgradeData(this);
            save.towerUpgradedata = towerUpgradeData;
            string newJson = JsonUtility.ToJson(save, true);
            File.WriteAllText(savePath, newJson);
        }
    }
    public void GetTowerPoint()
    {
        towerPoint++;
        if (totalMasteryPoint >= 30)
        {
            towerPoint = 0;
            return; 
        }
        if (towerPoint > 99)
        {
            totalMasteryPoint += 1;
            currentMasteryPoint += 1;
            towerPoint = 0;
        }
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
