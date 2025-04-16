using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTower : BaseTower
{
    [Header("����Ÿ�� ������")]
    [SerializeField] private LayerMask towerLayer;
    public IEffect buffTowerData;//��ó Ÿ���� �������� ����Ʈ �̸� ->IEffect���� �ٸ� �ŷ� ���� �ʿ�
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

        Debug.Log($"[BuffTower] �ֺ� Ÿ�� {nearbyTowers.Count}�� �߰�");
    }
}
