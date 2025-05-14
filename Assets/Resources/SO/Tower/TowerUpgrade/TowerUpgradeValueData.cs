using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TowerUpgradeValue
{
    public int index;
    public string upgradeName;
    public float[] levels;
    public TowerUpgradeValue(int index, string upgradeName, float lv0, float lv1, float lv2, float lv3)
    {
        this.index = index;
        this.upgradeName = upgradeName;
        this.levels = new float[] { lv0, lv1, lv2, lv3 };
    }
}

[CreateAssetMenu(fileName = "TowerUpgradeValueData", menuName = "Data/TowerUpgradeValueData")]
public class TowerUpgradeValueData : ScriptableObject
{
    [SerializeField] public List<TowerUpgradeValue> towerUpgradeValues;
    public void SetData(int index, string upgradeName, float lv0, float lv1, float lv2, float lv3)
    {
        TowerUpgradeValue towerUpgradeValue = new TowerUpgradeValue(index, upgradeName, lv0, lv1, lv2, lv3);
        towerUpgradeValues.Add(towerUpgradeValue);
    }
}
