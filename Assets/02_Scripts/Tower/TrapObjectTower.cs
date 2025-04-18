using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class TrapObjectTower : BaseTower
{
    [Header("오브젝트타워 데이터")]
    [SerializeField] private GameObject trapObjcetPrefab;
    private List<TrapObject> traps = new();
    private Vector2[] offsets = new Vector2[]
    {
        new Vector2(0, 1),       // 위
        new Vector2(-1, -1),     // 좌하단
        new Vector2(1, -1)       // 우하단
    };

    public override void Init(TowerData data)
    {
        base.Init(data);
        trapObjcetPrefab=TowerManager.Instance.TrapObjectPrefab;
        for (int i = 0; i < offsets.Length; i++)
        {
            // 설치 위치 확인
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