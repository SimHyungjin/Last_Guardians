using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TowerUpgrade :MonoBehaviour
{
    public TowerUpgradeData towerUpgradeData;
    private void Start()
    {
        towerUpgradeData = Resources.Load<TowerUpgradeData>("SO/Tower/TowerUpgrade/towerUpgrade");
        towerUpgradeData.Init();
    }
    public bool CanUpgrade(TowerUpgradeType type)
    {
        if (towerUpgradeData.currentMasteryPoint <= 0)
        {
            Debug.Log("마스터리 포인트가 부족합니다.");
            return false;
        }
        if (towerUpgradeData.upgradeData[type].currentLevel >= towerUpgradeData.upgradeData[type].maxLevel)
        {
            Debug.Log("업그레이드 최대 레벨에 도달했습니다.");
            return false;
        }
        if ((type == TowerUpgradeType.Penetration || type == TowerUpgradeType.ContinuousAttack ||
            type==TowerUpgradeType.Catalysis||type==TowerUpgradeType.EffectTransfer) 
            && towerUpgradeData.usedMasteryPoint < 10)
        {
            Debug.Log("해당 업그레이드를 진행하려면 사용 마스터리 포인트가 10을 초과해야 합니다.");
            return false;
        }
        if ((type == TowerUpgradeType.CombetMastery||type==TowerUpgradeType.MultipleAttack||type==TowerUpgradeType.BossSlayer)
            && towerUpgradeData.usedMasteryPoint < 20)
        {
            Debug.Log("해당 업그레이드를 진행하려면 사용 마스터리 포인트가 20을 초과해야 합니다.");
            return false;
        }
        if (towerUpgradeData.upgradeData.ContainsKey(type))
        {
            return towerUpgradeData.upgradeData[type].currentLevel < towerUpgradeData.upgradeData[type].maxLevel;
        }
        return false;
    }

    public void Upgrade(TowerUpgradeType type)
    {
        if (CanUpgrade(type))
        {
            towerUpgradeData.upgradeData[type].currentLevel++;
            towerUpgradeData.usedMasteryPoint += 1;
            towerUpgradeData.currentMasteryPoint--;
            SaveSystem.SaveTowerUpgradeData(towerUpgradeData.SetTowerUpgradeData());
            return;
        }
    }
}

