using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TrapObjectTower : BaseTower
{
    [Header("������ƮŸ�� ������")]
    [SerializeField] private LayerMask monsterLayer;

    private float lastCheckTime = 0f;

    public override void Init(TowerData data)
    {
        base.Init(data);
        monsterLayer = LayerMask.GetMask("Monster");
    }
}