using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private Transform[] target;
    [SerializeField] private List<MonsterWaveData> datas;

    public List<NormalEnemy> NormalPrefabs { get; private set; }
    public List<BossMonster> BossPrefabs { get; private set; }
    public List<BountyMonster> BountyPrefabs { get; private set; }

    //private BaseMonster monster;
    private int currentWaveIndex = 0;

    private int currentWaveMonsterCount = 0;
    private int spawnCount = 0;
    private int alliveCount = 0;

    private void Start()
    {
        datas.Sort((a, b) => a.WaveIndex.CompareTo(b.WaveIndex));
        InitMonsters();
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        if (currentWaveIndex >= datas.Count)
        {
            Debug.Log("모든 웨이브 완료");
            yield break;
        }

        MonsterWaveData wave = datas[currentWaveIndex];
        currentWaveMonsterCount = wave.Monster1Value + wave.Monster2Value + wave.Monster3Value + wave.Monster4Value;

        yield return new WaitForSeconds(wave.WaveStartDelay);
        Debug.Log($"웨이브 {wave.WaveIndex} 시작");
       
        yield return SpawnMonsters(wave);

        currentWaveIndex++;

        //StartCoroutine(StartNextWave());
    }

    private IEnumerator SpawnMonsters(MonsterWaveData wave)
    {
        for (int i = 0; i < wave.Monster1Value; i++)
        {
            SpawnMonster(wave.Monster1ID,wave.WaveIndex);
            yield return new WaitForSeconds(wave.MonsterSpawnInterval);
        }
        for (int i = 0; i < wave.Monster2Value; i++)
        {
            SpawnMonster(wave.Monster2ID, wave.WaveIndex);
            yield return new WaitForSeconds(wave.MonsterSpawnInterval);
        }
        for (int i = 0; i < wave.Monster3Value; i++)
        {
            SpawnMonster(wave.Monster3ID, wave.WaveIndex);
            yield return new WaitForSeconds(wave.MonsterSpawnInterval);
        }
        for (int i = 0; i < wave.Monster4Value; i++)
        {
            SpawnMonster(wave.Monster4ID, wave.WaveIndex);
            yield return new WaitForSeconds(wave.MonsterSpawnInterval);
        }
    }

    private void SpawnMonster(int monsterIndex, int waveLevel)
    {
        if(monsterIndex >=0 && monsterIndex <= 100)
        {
            NormalEnemy prefab = NormalPrefabs.Find(a => a.GetMonsterID() == monsterIndex);
            NormalEnemy monster = PoolManager.Instance.Spawn(prefab, spawnPoint[waveLevel % 2]);
            monster.Setup(prefab);
            monster.Target = waveLevel % 2 == 0 ? target[0] : target[1];
            Debug.Log($"몬스터 ID : {monster.GetMonsterID()}");
        }
        else if(monsterIndex >= 101 && monsterIndex <=200)
        {
            BossMonster prefab = BossPrefabs.Find(a => a.GetMonsterID() == monsterIndex);
            BossMonster monster = PoolManager.Instance.Spawn(prefab, spawnPoint[waveLevel % 2]);
            monster.Setup(prefab);
            monster.Target = waveLevel % 2 == 0 ? target[0] : target[1];
            Debug.Log($"몬스터 ID : {monster.GetMonsterID()}");
        }
            
        alliveCount++;
        spawnCount++;
    }

    public void OnMonsterDeath()
    {
        alliveCount--;
        if (alliveCount <= 0 && spawnCount == currentWaveMonsterCount)
        {
            Debug.Log("웨이브 클리어");
            spawnCount = 0;
            currentWaveMonsterCount = 0;
            StartCoroutine(StartNextWave());
        }
    }

    private void InitMonsters()
    {
        NormalPrefabs = Resources.LoadAll<NormalEnemy>("Enemy/Normal").ToList();
        NormalPrefabs.Sort((a, b) => a.GetMonsterID().CompareTo(b.GetMonsterID()));
        BossPrefabs = Resources.LoadAll<BossMonster>("Enemy/Boss").ToList();
        BossPrefabs.Sort((a, b) => a.GetMonsterID().CompareTo(b.GetMonsterID()));
        BountyPrefabs = Resources.LoadAll<BountyMonster>("Enemy/Bounty").ToList();
        BountyPrefabs.Sort((a, b) => a.GetMonsterID().CompareTo(b.GetMonsterID()));
    }

    public void TestKill()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject obj2 in obj)
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
