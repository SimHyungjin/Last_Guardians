using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;   

public class InGameManager : Singleton<InGameManager>
{
    public ObstacleContainer obstacleContainer;
    public List<TowerData> TowerDatas { get; private set; }

    private int playerMaxHP = 100;
    public int PlayerHP { get; private set; }
    public DamageText DamageTextPrefab { get; private set; }
    public Canvas DamageUICanvas { get; private set; }

    private Transform target;
    private Canvas damageUICanvasPrefab;

    [SerializeField] private MulliganUI mulliganUI;
    [SerializeField] private Image playerHPbar;
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private Image playerEXPbar;
    [SerializeField] private TextMeshProUGUI playerEXPText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI waveInfoText;
    [SerializeField] private TextMeshProUGUI remainMonsterCountText;
    [SerializeField] private GameOverUI gameoverUI;
    [SerializeField] private TextMeshProUGUI weatherInfo;
    [SerializeField] private GameObject mapSlot;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private HitEffectUI hitEffectUI;

    public List<GameObject> MapPrefabs { get; private set; } = new();
    public Tilemap ObstacleTilemap { get; private set; }  

    public int level;
    public float exp;
    private double maxExp = 50;

    private GameObject map;

    public bool isDie = false;
    public bool isGameOver = false;

    private void Awake()
    {
        InItTowerData();
        PlayerHP = playerMaxHP;
    }

    private void Start()
    {
        obstacleContainer = new ObstacleContainer();
        GameManager.Instance.PlayerManager.SpawnPlayerModel();

        PrefabInit();

        if (DamageUICanvas == null)
            DamageUICanvas = Instantiate(damageUICanvasPrefab);

        UpdateHP();
        UpdateExp();
        exp = 0;

        DateTime now = DateTime.Now;
        GameManager.Instance.NowTime = now.Minute;
        EnviromentManager.Instance.SetSeason(GameManager.Instance.NowTime);

        // 맵 동적 생성
        if (EnviromentManager.Instance.Season != Season.winter)
            map = Instantiate(MapPrefabs[0], mapSlot.transform);
        else
            map = Instantiate(MapPrefabs[1], mapSlot.transform);

       
        ObstacleTilemap = map.GetComponentInChildren<Tilemap>();

        // 기존 로직
        target = map.transform.Find("Center");
        MonsterManager.Instance.Target = target;
        TowerManager.Instance.towerbuilder.targetPosition = target;
        if(PlayerPrefs.GetInt("InGameTutorial")!=1)
            tutorial.gameObject.SetActive(true);
        else MuliigunStart();
        //mulliganUI.StartSelectCard();
    }

    public void GetExp(float exp)
    {
        this.exp += exp;
        UpdateExp();
        SoundManager.Instance.PlaySFX("EXP");
        if (this.exp >= maxExp)
            LevelUp();
    }

    public void LevelUp()
    {
        Time.timeScale = 0f;
        level++;
        exp = (float)(exp - maxExp);
        maxExp = 13 + 14 * Math.Pow(level - 1, 0.98);
        UpdateExp();
        levelText.text = $"Lv {level}";
        SoundManager.Instance.PlaySFX("LevelUp");
        TowerManager.Instance.StartInteraction(InteractionState.Pause);
        if(TowerManager.Instance.hand.IsHighlighting)TowerManager.Instance.hand.CancleCard();
        mulliganUI.gameObject.SetActive(true);
        mulliganUI.LevelUPSelect();
        
    }

    private void InItTowerData()
    {
        TowerDatas = Resources.LoadAll<TowerData>("SO/Tower").ToList();
        TowerDatas.Sort((a, b) => a.TowerIndex.CompareTo(b.TowerIndex));
    }
    public void MuliigunStart()
    {
        mulliganUI.StartSelectCard();
    }

    public void GameStart()
    {
        MonsterManager.Instance.GameStart();
        GetExp((float)maxExp);
      
    }

    public void AddCardTOHand(int index)
    {
        TowerManager.Instance.hand.AddCard(index);
        TowerManager.Instance.EndInteraction(InteractionState.Pause);
    }

    public void TakeDmage(int amount)
    {
        PlayerHP = Mathf.Max(0, PlayerHP - amount);
        hitEffectUI.PlayHitEffect();
        UpdateHP();
        if (PlayerHP <= 0)
        {
            isDie = true;
            GameOver();
        }
            
    }

    private void PrefabInit()
    {
        DamageTextPrefab = Resources.Load<DamageText>("UI/DamageUI/DamageIndicator");
        damageUICanvasPrefab = Resources.Load<Canvas>("UI/DamageUI/DamageCanvas");

        MapPrefabs.Add(Resources.Load<GameObject>("Enviroment/Maps/MapBase"));
        MapPrefabs.Add(Resources.Load<GameObject>("Enviroment/Maps/MapWinter"));
    }

    private void GameOver()
    {
        if (gameoverUI.gameObject.activeSelf) return;
        isGameOver = true;

        foreach(var tower in TowerManager.Instance.Towers)
        {
            if(tower is AttackTower)
            {
                tower.OnDisabled();
            }
        }
        MonsterManager.Instance.StopAllCoroutines();
        TowerManager.Instance.StartInteraction(InteractionState.Pause);
        TowerManager.Instance.hand.gameObject.SetActive(false);
        gameoverUI.gameObject.SetActive(true);
        TowerManager.Instance.towerUpgradeData.Save();

        AnalyticsLogger.LogWaveEnd(isDie,MonsterManager.Instance.nowWave.WaveIndex);
        Time.timeScale = 0f;
    }

    public void GameExit() => GameOver();

    public void SetWaveInfoText(int wave, int count)
    {
        waveInfoText.text = $"{wave} Wave";
        remainMonsterCountText.text = $"남은 몬스터 수 {count}";
    }

    private void UpdateHP()
    {
        playerHPbar.fillAmount = (float)PlayerHP / playerMaxHP;
        playerHPText.text = $"현재 체력 : {PlayerHP} / {playerMaxHP}";
    }

    private void UpdateExp()
    {
        playerEXPbar.fillAmount = (float)(exp / maxExp);
        playerEXPText.text = $"경험치 : {(int)exp} / {(int)maxExp}";
    }

    public void UpdateWeatherInfo()
    {
        weatherInfo.text = $"{EnviromentManager.Instance.WeatherState.GetSeasonText("")} / {EnviromentManager.Instance.WeatherState.GetWeatherName()}";
    }

    public double GetMaxExp()
    {
        return maxExp;
    }
}
