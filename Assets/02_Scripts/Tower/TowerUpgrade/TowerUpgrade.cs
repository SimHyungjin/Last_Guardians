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
            Debug.Log("�����͸� ����Ʈ�� �����մϴ�.");
            return false;
        }
        if (towerUpgradeData.upgradeData[type].currentLevel >= towerUpgradeData.upgradeData[type].maxLevel)
        {
            Debug.Log("���׷��̵� �ִ� ������ �����߽��ϴ�.");
            return false;
        }
        if ((type == TowerUpgradeType.Penetration || type == TowerUpgradeType.ContinuousAttack ||
            type==TowerUpgradeType.Catalysis||type==TowerUpgradeType.EffectTransfer) 
            && towerUpgradeData.usedMasteryPoint < 10)
        {
            Debug.Log("�ش� ���׷��̵带 �����Ϸ��� ��� �����͸� ����Ʈ�� 10�� �ʰ��ؾ� �մϴ�.");
            return false;
        }
        if ((type == TowerUpgradeType.CombetMastery||type==TowerUpgradeType.MultipleAttack||type==TowerUpgradeType.BossSlayer)
            && towerUpgradeData.usedMasteryPoint < 20)
        {
            Debug.Log("�ش� ���׷��̵带 �����Ϸ��� ��� �����͸� ����Ʈ�� 20�� �ʰ��ؾ� �մϴ�.");
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

