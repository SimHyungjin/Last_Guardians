using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private Transform[] target;

    public List<MonsterWaveData> WaveDatas { get; private set; }
    public NormalEnemy NormalPrefab { get; private set; }
    public BossMonster BossPrefab { get; private set; }
    public BountyMonster BountyPrefab { get; private set; }
    public EnemyProjectile ProjectilePrefab { get; private set; }
    public List<MonsterData> MonsterDatas { get; private set; }
    public List<MonsterSkillBase> MonsterSkillDatas { get; private set; }
    //public List<BaseMonster> AlliveMonsters { get; private set; }
    public MonsterWaveData nowWave { get; private set; }
    public EXPBead EXPBeadPrefab { get; private set; }
    public List<MonsterData> BountyMonsterList {  get; private set; }
    public float BountySpwanCoolTime { get; private set; }
    public float SpawnTimer { get; set; }
    private Coroutine spawnTimerCorutine;
    private WaitForSeconds spawnSeconds;
    public Action spawnAction;

    public int currentWaveIndex { get; private set; } = 0;
    private int currentWaveMonsterCount = 0;
    private int spawnCount = 0;
    private int alliveCount = 0;
    public int RemainMonsterCount {  get; private set; }
    public int MonsterKillCount { get; set; }
    private void Start()
    {
        //AlliveMonsters = new List<BaseMonster>();
        spawnSeconds = new WaitForSeconds(0.1f);
        InitMonsters();
    }

    public void GameStart()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        if (currentWaveIndex >= WaveDatas.Count)
        {
            Debug.Log("모든 웨이브 완료");
            yield break;
        }

        nowWave = WaveDatas[currentWaveIndex];
        currentWaveMonsterCount = nowWave.Monster1Value + nowWave.Monster2Value + nowWave.Monster3Value + nowWave.Monster4Value;
        RemainMonsterCount = currentWaveMonsterCount;

        yield return new WaitForSeconds(nowWave.WaveStartDelay);
        Debug.Log($"웨이브 {nowWave.WaveIndex} 시작");
        InGameManager.Instance.SetWaveInfoText(nowWave.WaveIndex, RemainMonsterCount);

        yield return SpawnMonsters(nowWave);


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
            MonsterData data = MonsterDatas.Find(a => a.MonsterIndex == monsterIndex);
            NormalEnemy monster = PoolManager.Instance.Spawn(NormalPrefab, spawnPoint[waveLevel % 2]);
            monster.Setup(data);
            monster.Target = waveLevel % 2 == 0 ? target[0] : target[1];
            alliveCount++;
            spawnCount++;
            //Debug.Log($"몬스터 ID : {monster.GetMonsterID()}");
        }
        else if(monsterIndex >= 101 && monsterIndex <=200)
        {
            MonsterData data = MonsterDatas.Find(a => a.MonsterIndex == monsterIndex);
            BossMonster monster = PoolManager.Instance.Spawn(BossPrefab, spawnPoint[waveLevel % 2]);
            monster.Setup(data);
            monster.Target = waveLevel % 2 == 0 ? target[0] : target[1];
            //AlliveMonsters.Add(monster);
            alliveCount++;
            spawnCount++;
            //Debug.Log($"몬스터 ID : {monster.GetMonsterID()}");
        }
        else if(monsterIndex >= 201 && monsterIndex <=300)
        {
            MonsterData data = MonsterDatas.Find(a => a.MonsterIndex == monsterIndex);
            BossMonster monster = PoolManager.Instance.Spawn(BossPrefab, spawnPoint[waveLevel % 2]);
            monster.Setup(data);
            monster.Target = waveLevel % 2 == 0 ? target[0] : target[1];
            //AlliveMonsters.Add(monster);
        }
        
    }
    public void StartSpawnTimer()
    {
        if (spawnTimerCorutine == null)
        {
            spawnTimerCorutine = StartCoroutine(SpawnTimerCoroutine());
        }
    }

    IEnumerator SpawnTimerCoroutine()
    {
        SpawnTimer = BountySpwanCoolTime;

        while (SpawnTimer > 0f)
        {
            SpawnTimer -= Time.deltaTime;
            yield return spawnSeconds;
        }

        spawnTimerCorutine = null;
    }

    public void SpawnBounty(int index)
    {   
        if (index >= 201 && index <= 300)
        {
            MonsterData data = MonsterDatas.Find(a => a.MonsterIndex == index);
            BountyMonster monster = PoolManager.Instance.Spawn(BountyPrefab, spawnPoint[nowWave.WaveLevel % 2]);
            monster.Setup(data);
            monster.Target = nowWave.WaveLevel % 2 == 0 ? target[0] : target[1];
            //AlliveMonsters.Add(monster);
        }
    }

    public void OnMonsterDeath(BaseMonster monster)
    {
        alliveCount--;
        RemainMonsterCount--;
        MonsterKillCount++;
        InGameManager.Instance.SetWaveInfoText(nowWave.WaveIndex, RemainMonsterCount);

        if (alliveCount <= 0 && spawnCount == currentWaveMonsterCount)
        {
            if (InGameManager.Instance.PlayerHP <= 0)
                return;
            Debug.Log("웨이브 클리어");
            currentWaveIndex++;
            spawnCount = 0;
            currentWaveMonsterCount = 0;
            StartCoroutine(StartNextWave());
        }
    }

    private void InitMonsters()
    {
        NormalPrefab = Resources.Load<NormalEnemy>("Enemy/Normal/NormalMonster");
        BossPrefab = Resources.Load<BossMonster>("Enemy/Boss/BossMonster");
        BountyPrefab = Resources.Load<BountyMonster>("Enemy/Bounty/BountyMonster");
        MonsterDatas = Resources.LoadAll<MonsterData>("SO/Enemy/MonsterSO").ToList();
        MonsterDatas.Sort((a, b) => a.MonsterIndex.CompareTo(b.MonsterIndex));
        MonsterSkillDatas = Resources.LoadAll<MonsterSkillBase>("Enemy/MonsterSkillPrefab").ToList();
        MonsterSkillDatas.Sort((a, b) => a.skillData.SkillIndex.CompareTo(b.skillData.SkillIndex));
        WaveDatas = Resources.LoadAll<MonsterWaveData>("SO/Enemy/MonsterWaveSOData").ToList();
        WaveDatas.Sort((a, b) => a.WaveIndex.CompareTo(b.WaveIndex));
        ProjectilePrefab = Resources.Load<EnemyProjectile>("Enemy/EnemyProjectile/EnemyProjectile");

        EXPBeadPrefab = Resources.Load<EXPBead>("Enemy/EXPBead");

        BountyMonsterList = MonsterDatas.FindAll(a => a.MonsterIndex >= 201 && a.MonsterIndex <= 300);

        BountySpwanCoolTime = 60f;
        SpawnTimer = 0f;
    }
    public void ForceAddWave()
    {
        currentWaveIndex++;
        Debug.Log($"[테스트] 현재 웨이브 → {currentWaveIndex}");
    }

}
