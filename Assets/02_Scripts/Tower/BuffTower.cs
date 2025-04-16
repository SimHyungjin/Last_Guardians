using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTower : BaseTower
{
    [Header("버프타워 데이터")]
    [SerializeField] private LayerMask towerLayer;
    public IEffect buffTowerData;//근처 타워에 전달해줄 이펙트 이름 ->IEffect에서 다른 거로 변경 필요
    private List<BaseTower> nearbyTowers = new();
    public override void Init(int index)
    {
        base.Init(index);
        ApplyBuffOnPlacement();
    }

    private void ApplyBuffOnPlacement()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange, towerLayer);

        nearbyTowers.Clear();

        foreach (var hit in hits)
        {
            BaseTower otherTower = hit.GetComponent<BaseTower>();
            if (otherTower != null && otherTower != this)
            {
                nearbyTowers.Add(otherTower);
            }
        }

        Debug.Log($"[BuffTower] 주변 타워 {nearbyTowers.Count}개 발견");
    }
}
