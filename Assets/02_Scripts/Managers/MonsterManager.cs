using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterManager : Singleton<MonsterManager>
{
    [SerializeField] private Transform[] spawnPoint;
    //[SerializeField] private Transform[] target;
    [SerializeField] private RuntimeAnimatorController normalAnim;
    [SerializeField] private RuntimeAnimatorController horseAnim;

    public Transform Target { get; set; }
    public int MaxWave { get; private set; } = 0;
    public List<MonsterWaveData> WaveDatas { get; private set; }
    public NormalEnemy NormalPrefab { get; private set; }
    public BossMonster BossPrefab { get; private set; }
    public BountyMonster BountyPrefab { get; private set; }
    public EnemyProjectile ProjectilePrefab { get; private set; }
    public List<MonsterData> MonsterDatas { get; private set; }
    public List<MonsterSkillBase> MonsterSkillDatas { get; private set; }
    public List<BaseMonster> AlliveMonsters { get; private set; }
    public MonsterWaveData nowWave { get; private set; }
    public EXPBead EXPBeadPrefab { get; private set; }
    public List<MonsterData> BountyMonsterList { get; private set; }
    public float BountySpwanCoolTime { get; private set; }
    public int BossKillCount { get; set; }
    public float SpawnTimer { get; set; }
    private Coroutine spawnTimerCorutine;
    private WaitForSeconds spawnSeconds;
    public Action spawnAction;

    public int currentWaveIndex { get; private set; } = 0;
    private int currentWaveMonsterCount = 0;
    private int spawnCount = 0;
    private int alliveCount = 0;
    public int RemainMonsterCount { get; private set; }
    public int MonsterKillCount { get; set; }
    public int EXPCount { get; set; } = 0;

    public int Catalysis { get; private set; }
    public float CatalysisValue { get; private set; }
    public int EffectTransfer { get; private set; }
    public float EffectTransferUpgradeValue { get; private set; }

    public int WaveLevel { get; private set; } = 0;

    private Dictionary<int, MonsterData> monsterDataDict;
    private void Start()
    {
        AlliveMonsters = new List<BaseMonster>();
        spawnSeconds = new WaitForSeconds(0.1f);
        InitMonsters();
        //AnimationConnect.AddAnimationEvent(normalAnim);
        //AnimationConnect.AddAnimationEvent(horseAnim);
        CacheMonsterData();

        MaxWave = PlayerPrefs.GetInt("IdleMaxWave", 0);
    }

    public void GameStart()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        if (currentWaveIndex == WaveDatas.Count)
        {
            currentWaveIndex = 0;
            //yield break;
        }

        WaveLevel++;
        nowWave = WaveDatas[currentWaveIndex];
        currentWaveMonsterCount = nowWave.Monster1Value + nowWave.Monster2Value + nowWave.Monster3Value + nowWave.Monster4Value;
        RemainMonsterCount = currentWaveMonsterCount;

        yield return new WaitForSeconds(nowWave.WaveStartDelay);

        if (nowWave.WaveIndex % EnviromentManager.Instance.WeatherCycle == 1)
        {
            EnviromentManager.Instance.WeatherState.SetWeather(); // 다음 날씨 시작
            InGameManager.Instance.UpdateWeatherInfo();
        }


        InGameManager.Instance.SetWaveInfoText(WaveLevel, RemainMonsterCount);

        yield return SpawnMonsters(nowWave);

        //StartCoroutine(StartNextWave());
    }

    private void CacheMonsterData()
    {
        monsterDataDict = new Dictionary<int, MonsterData>();
        foreach (var data in MonsterDatas)
        {
            if (!monsterDataDict.ContainsKey(data.MonsterIndex))
                monsterDataDict.Add(data.MonsterIndex, data);
        }
    }

    private IEnumerator SpawnMonsters(MonsterWaveData wave)
    {
        for (int i = 0; i < wave.Monster1Value; i++)
        {
            SpawnMonster(wave.Monster1ID, wave.WaveIndex);
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
        if (!monsterDataDict.TryGetValue(monsterIndex, out var data))
        {
            return;
        }

        BaseMonster monster = null;

        if (monsterIndex >= 0 && monsterIndex <= 100)
        {
            monster = PoolManager.Instance.Spawn(NormalPrefab, spawnPoint[waveLevel % 2]);
        }
        else if (monsterIndex >= 101 && monsterIndex <= 300)
        {
            monster = PoolManager.Instance.Spawn(BossPrefab, spawnPoint[waveLevel % 2]);
        }

        if (monster != null)
        {
            monster.Setup(data);
            monster.Target = Target;
            AlliveMonsters.Add(monster);
            alliveCount++;
            spawnCount++;
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
            BountyMonster monster = PoolManager.Instance.Spawn(BountyPrefab, spawnPoint[WaveLevel % 2]);
            monster.Setup(data);
            monster.Target = Target;
            AlliveMonsters.Add(monster);
        }
    }

    public BaseMonster SummonMonster(int index, Vector2 pos)
    {

        MonsterData data = MonsterDatas.Find(a => a.MonsterIndex == index);
        NormalEnemy monster = PoolManager.Instance.SpawnbyPrefabName(NormalPrefab);
        if (monster.agent != null)
        {
            monster.agent.enabled = false;
            monster.transform.position = pos;
            monster.agent.enabled = true;
            monster.agent.Warp(pos);
        }
        else
        {
            monster.transform.position = pos;
        }
        monster.Setup(data);
        //monster.transform.position = pos;
        monster.transform.SetParent(PoolManager.Instance.transform);
        AlliveMonsters.Add(monster);
        alliveCount++;
        spawnCount++;
        RemainMonsterCount++;
        currentWaveMonsterCount++;
        InGameManager.Instance.SetWaveInfoText(WaveLevel, RemainMonsterCount);

        return monster;
    }

    public void OnMonsterDeath(BaseMonster monster)
    {
        if (monster.MonsterData.MonsterType != MonType.Bounty)
        {
            RemainMonsterCount--;
            alliveCount--;
        }

        if (AlliveMonsters.Contains(monster))
        {
            AlliveMonsters.Remove(monster);
        }

        InGameManager.Instance.SetWaveInfoText(WaveLevel, RemainMonsterCount);

        if (IsWaveComplete())
        {
            if (InGameManager.Instance.PlayerHP <= 0)
                return;


            currentWaveIndex++;

            if (currentWaveIndex > MaxWave)
            {
                MaxWave = WaveLevel;
                PlayerPrefs.SetInt("IdleMaxWave", MaxWave);
                PlayerPrefs.Save();
            }

            spawnCount = 0;
            currentWaveMonsterCount = 0;
            RemainMonsterCount = 0;
            alliveCount = 0;

            StartCoroutine(StartNextWave());
        }
    }

    private bool IsWaveComplete()
    {
        return alliveCount <= 0 && spawnCount == currentWaveMonsterCount;
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

        Catalysis = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.Catalysis];
        CatalysisValue = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.Catalysis].levels[Catalysis];
        EffectTransfer = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.EffectTransfer];
        EffectTransferUpgradeValue = TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.EffectTransfer].levels[EffectTransfer];
    }

}
