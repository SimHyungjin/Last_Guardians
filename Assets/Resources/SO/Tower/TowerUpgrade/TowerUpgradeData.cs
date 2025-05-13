using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SerializableUpgradeData
{
    public int totalMasteryPoint;
    public int currentMasteryPoint;
    public int usedMasteryPoint;
    public string[] upgradeType;
    public int[] currentLevel; 
}

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
    [Header("�����͸� ����Ʈ")]
    public int totalMasteryPoint;
    public int currentMasteryPoint;
    public int usedMasteryPoint;
    public Dictionary<TowerUpgradeType, TowerUpgradeInfo> upgradeData;

    [Header("����ũ��Ʈ")]
    public string[] description;
    public void Init()
    {
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
            Debug.Log("[TowerUpgradeData] save.json�� ��� ���� ������.");
        }
        else
        {
            string json = File.ReadAllText(savePath);
            var save = JsonUtility.FromJson<SaveData>(json);
            LoadTowerUpgradeData(save.TowerUpgradeData);
        }
        usedMasteryPoint = totalMasteryPoint - currentMasteryPoint;
    }

    public SerializableUpgradeData SetTowerUpgradeData()
    {
        int Length = Enum.GetValues(typeof(TowerUpgradeType)).Length;   
        SerializableUpgradeData serializableUpgradeData = new SerializableUpgradeData();
        serializableUpgradeData.upgradeType = new string[Length];
        serializableUpgradeData.currentLevel = new int[Length];
        serializableUpgradeData.totalMasteryPoint = totalMasteryPoint;
        serializableUpgradeData.currentMasteryPoint = currentMasteryPoint;
        serializableUpgradeData.usedMasteryPoint = usedMasteryPoint;
        for(int i = 0; i < Length; i++)
        {
            TowerUpgradeType type = (TowerUpgradeType)i;
            serializableUpgradeData.upgradeType[i] = type.ToString();
            serializableUpgradeData.currentLevel[i] = upgradeData[type].currentLevel;
        }
        return serializableUpgradeData;
    }

    public void LoadTowerUpgradeData(SerializableUpgradeData serializableUpgradeData)
    {
        int Length = Enum.GetValues(typeof(TowerUpgradeType)).Length;
        totalMasteryPoint = serializableUpgradeData.totalMasteryPoint;
        currentMasteryPoint = serializableUpgradeData.currentMasteryPoint;
        usedMasteryPoint = serializableUpgradeData.usedMasteryPoint;
        upgradeData.Clear();
        for (int i = 0; i < Length; i++)
        {
            TowerUpgradeType type = (TowerUpgradeType)Enum.Parse(typeof(TowerUpgradeType), serializableUpgradeData.upgradeType[i]);
            if (upgradeData.ContainsKey(type))
            {
                upgradeData[type].currentLevel = serializableUpgradeData.currentLevel[i];
            }
            else
            {
                upgradeData.Add(type, new TowerUpgradeInfo { currentLevel = serializableUpgradeData.currentLevel[i] });
            }
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
