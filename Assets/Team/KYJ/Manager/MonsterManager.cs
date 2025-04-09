using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{

    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private Transform target;
    [SerializeField] private MonsterWaveData[] datas;

    private BaseMonster monster;
    private float SpawnTimer = 0f;
    private float SpawnDelay = 5f;


    private void Update()
    {
        if(SpawnTimer <=0)
            SpawnMonster();

        SpawnTimer -= Time.deltaTime;
    }

    private void SpawnMonster()
    {
        SpawnMonster(spawnPoint[Random.Range(0, 2)],"Boss","BossEnemy");
    }

    private void SpawnMonster(Transform point,string grade ,string name)
    {
        BaseMonster prefab = Resources.Load<BaseMonster>($"Enemy/{grade}/{name}");
        BaseMonster monster = PoolManager.Instance.Spawn(prefab, point);
        monster.Target = target;
        SpawnTimer = SpawnDelay;
    }

    public void TestKill()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Monster");
        foreach(GameObject obj2 in obj)
        {
            BaseMonster ba = obj2.GetComponent<BaseMonster>();
            ba.TakeDamage(1000);
        }
    }

    public void TestDot()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject obj2 in obj)
        {
            BaseMonster ba = obj2.GetComponent<BaseMonster>();
            ba.DotDamage(5, 10f);
        }
    }

    public void TestSturn()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject obj2 in obj)
        {
            BaseMonster ba = obj2.GetComponent<BaseMonster>();
            ba.ApplySturn(10f, 5f);
        }
    }
}
