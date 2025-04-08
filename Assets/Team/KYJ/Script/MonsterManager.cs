using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{

    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private Transform target;

    private BaseMonster monster;
    private float SpawnTimer = 0f;
    private float SpawnDelay = 5f;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(SpawnTimer <=0)
            SpawnMonster();

        SpawnTimer -= Time.deltaTime;
    }

    private void SpawnMonster()
    {
        SpawnNormalMonster(spawnPoint[Random.Range(0, 2)]);
    }

    private void SpawnNormalMonster(Transform point)
    {
        NormalEnemy prefab = Resources.Load<NormalEnemy>("Enemy/Normal/Enemy");
        NormalEnemy normalEnemy = PoolManager.Instance.Spawn(prefab, point);
        normalEnemy.Target = target;
        SpawnTimer = SpawnDelay;
    }
}
