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
        if (towerUpgradeData.currentMasteryPoint <= 0)
        {
            Debug.Log("마스터리 포인트가 부족합니다.");
            return false;
        }
        if (towerUpgradeData.currentLevel[(int)type] >= towerUpgradeData.maxLevel)
        {
            Debug.Log("업그레이드 최대 레벨에 도달했습니다.");
            return false;
        }
        if ((type == TowerUpgradeType.Penetration || type == TowerUpgradeType.ContinuousAttack ||
            type==TowerUpgradeType.Catalysis||type==TowerUpgradeType.EffectTransfer) 
            && towerUpgradeData.SumOfLevelUP < 10 && towerUpgradeData.currentMasteryPoint<2)
        {
            Debug.Log("해당 업그레이드를 진행하려면 사용 마스터리 포인트가 10을 초과해야 합니다.");
            Debug.Log("현재 마스터리 포인트가 부족합니다.");
            return false;
        }
        if ((type == TowerUpgradeType.CombetMastery||type==TowerUpgradeType.MultipleAttack||type==TowerUpgradeType.BossSlayer)
            && towerUpgradeData.SumOfLevelUP < 20 && towerUpgradeData.currentMasteryPoint < 5)
        {
            Debug.Log("해당 업그레이드를 진행하려면 사용 마스터리 포인트가 20을 초과해야 합니다.");
            return false;
        }
        else
        {
            return towerUpgradeData.currentLevel[(int)type] < towerUpgradeData.maxLevel;
        }
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

