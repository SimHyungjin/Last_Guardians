using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TrapObjectTower : BaseTower
{
    [Header("오브젝트타워 데이터")]
    [SerializeField] private LayerMask monsterLayer;

    private float lastCheckTime = 0f;

    public override void Init(TowerData data)
    {
        base.Init(data);
        monsterLayer = LayerMask.GetMask("Monster");
    }
}