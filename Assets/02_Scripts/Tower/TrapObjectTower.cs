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

    /// <summary>
    /// ������ ��ġ�� ������Ʈ Ʈ���� ��ġ
    /// </summary>
    /// <param name="data"></param>
    public override void Init(TowerData data)
    {
        base.Init(data);
        trapObjcetPrefab=TowerManager.Instance.TrapObjectPrefab;
        for (int i = 0; i < offsets.Length; i++)
        {
            Vector2 worldPos = (Vector2)transform.position + offsets[i];

            GameObject trapGO = Instantiate(trapObjcetPrefab, transform);
            trapGO.transform.position = worldPos;
            TrapObject trap = trapGO.GetComponent<TrapObject>();
            trap.Init(towerData);
            traps.Add(trap);
        }
    }

    /// <summary>
    /// �ı��� Ʈ�� ������Ʈ�� ��� �ı�
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
        foreach (TrapObject trap in traps)
        {
            Destroy(trap.gameObject);
        }
    }
}