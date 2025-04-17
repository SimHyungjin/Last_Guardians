using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class TrapObjectTower : BaseTower
{
    [Header("������ƮŸ�� ������")]
    [SerializeField] private GameObject trapObjcetPrefab;
    private List<TrapObject> traps = new();
    private Vector2[] offsets = new Vector2[]
    {
        new Vector2(0, 1),       // ��
        new Vector2(-1, -1),     // ���ϴ�
        new Vector2(1, -1)       // ���ϴ�
    };

    public override void Init(TowerData data)
    {
        base.Init(data);
        trapObjcetPrefab=TowerManager.Instance.TrapObjectPrefab;
        for (int i = 0; i < offsets.Length; i++)
        {
            // ��ġ ��ġ Ȯ��
            Vector2 worldPos = (Vector2)transform.position + offsets[i];

            GameObject trapGO = Instantiate(trapObjcetPrefab, transform);
            trapGO.transform.position = worldPos;
            TrapObject trap = trapGO.GetComponent<TrapObject>();
            trap.Init(towerData);
            traps.Add(trap);
        }
    }

    protected override void OnDestroy()
    {
        foreach (TrapObject trap in traps)
        {
            Destroy(trap.gameObject);
        }
    }
}