using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    public TowerUpgradeData towerUpgradeData;

    void Awake()
    {
        towerUpgradeData.Init(Application.persistentDataPath + "/save.json");
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
