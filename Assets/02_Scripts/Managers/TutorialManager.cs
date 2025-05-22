using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public ObstacleContainer obstacleContainer;
    public List<TowerData> TowerDatas { get; private set; }

    private int playerMaxHP = 100;
    public int PlayerHP { get; private set; }

    private Transform target;

    public bool isTutorial = true;

    public float TimeScale { get; private set; } = 1;

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
        
        UpdateHP();
        UpdateExp();
        exp = 0;
        EnviromentManager.Instance.SetSeason(10);

        ObstacleTilemap = map.GetComponentInChildren<Tilemap>();

        // 기존 로직
        target = map.transform.Find("Center");
        MonsterManager.Instance.Target = target;
        TowerManager.Instance.towerbuilder.targetPosition = target;
        MuliigunStart();
        //mulliganUI.StartSelectCard();
    }

    private void InItTowerData()
    {
        TowerDatas = Resources.LoadAll<TowerData>("SO/Tower").ToList();
        TowerDatas.Sort((a, b) => a.TowerIndex.CompareTo(b.TowerIndex));
    }

    private void PrefabInit()
    {
        damageUICanvasPrefab = Resources.Load<Canvas>("Prefabs/UI/DamageUICanvas");
        MapPrefabs.AddRange(Resources.LoadAll<GameObject>("Prefabs/Map"));
        obstacleContainer = new ObstacleContainer();
    }
    public void UpdateHP()
    {
        playerHPbar.fillAmount = (float)PlayerHP / playerMaxHP;
        playerHPText.text = PlayerHP.ToString();
    }

    public void UpdateExp()
    {
        playerEXPbar.fillAmount = (float)exp / (float)maxExp;
        playerEXPText.text = exp.ToString();
        levelText.text = "Level : " + level.ToString();
    }
    public void MuliigunStart()
    {
        mulliganUI.StartSelectCard();
    }
}