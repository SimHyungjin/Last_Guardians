using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[System.Serializable]
public class TowerUpgrade :MonoBehaviour
{
    public TowerUpgradeData towerUpgradeData;
    private void Start()
    {
        towerUpgradeData = Resources.Load<TowerUpgradeData>("SO/Tower/TowerUpgrade/TowerUpgrade");
        towerUpgradeData.Init();
    }
    public bool CanUpgrade(TowerUpgradeType type)
    {
        if (towerUpgradeData.currentLevel[(int)type] >= towerUpgradeData.maxLevel)
        {
            return false;
        }
        if (IsTier3(type))
        {
            if (towerUpgradeData.SumOfLevelUP < 20)
            {
                return false;
            }
            if (towerUpgradeData.currentMasteryPoint < 5)
            {
                return false;
            }
        }
        else if (IsTier2(type))
        {
            if (towerUpgradeData.SumOfLevelUP < 10)
            {
                return false;
            }
            if (towerUpgradeData.currentMasteryPoint < 2)
            {
                return false;
            }
        }
        else if (towerUpgradeData.currentMasteryPoint <= 0)
        {
            return false;
        }
        return true;
    }

    public void Upgrade(TowerUpgradeType type)
    {
        if (CanUpgrade(type))
        {
            towerUpgradeData.currentLevel[(int)type]++;
            towerUpgradeData.SumOfLevelUP++;
            if (IsTier1(type))
            {
                towerUpgradeData.usedMasteryPoint += 1;
                towerUpgradeData.currentMasteryPoint -= 1;
            }
            else if (IsTier2(type))
            {
                towerUpgradeData.usedMasteryPoint += 2;
                towerUpgradeData.currentMasteryPoint -= 2;
            }
            else if (IsTier3(type))
            {
                towerUpgradeData.usedMasteryPoint += 5;
                towerUpgradeData.currentMasteryPoint -= 5;
            }
            else
            {
                Debug.Log("업그레이드 실패");
                return;
            }
            SaveSystem.SaveTowerUpgradeData(this);

            AnalyticsLogger.LogTowerUpgrade(towerUpgradeData);
            return;
        }
    }

    public void Reset()
    {
        towerUpgradeData.currentLevel = new List<int>(new int[towerUpgradeData.currentLevel.Count]);
        for (int i = 0; i < towerUpgradeData.currentLevel.Count; i++)
        {
            towerUpgradeData.currentLevel[i] = 0;
        }
        towerUpgradeData.SumOfLevelUP = 0;
        towerUpgradeData.usedMasteryPoint = 0;
        towerUpgradeData.currentMasteryPoint = towerUpgradeData.totalMasteryPoint;
        SaveSystem.SaveTowerUpgradeData(this);
    }

    private bool IsTier1(TowerUpgradeType type)
    {
        return type == TowerUpgradeType.AttackPower ||
               type == TowerUpgradeType.AttackSpeed ||
               type == TowerUpgradeType.AttackRange ||
               type == TowerUpgradeType.EffectValue ||
               type == TowerUpgradeType.EffectDuration ||
               type == TowerUpgradeType.EffectRange;
    }
    private bool IsTier2(TowerUpgradeType type)
    {
        return type == TowerUpgradeType.Penetration ||
               type == TowerUpgradeType.ContinuousAttack ||
               type == TowerUpgradeType.Catalysis ||
               type == TowerUpgradeType.EffectTransfer;
    }

    private bool IsTier3(TowerUpgradeType type)
    {
        return type == TowerUpgradeType.CombetMastery ||
               type == TowerUpgradeType.MultipleAttack ||
               type == TowerUpgradeType.BossSlayer ||
               type == TowerUpgradeType.Emergencyresponse;
    }
}

