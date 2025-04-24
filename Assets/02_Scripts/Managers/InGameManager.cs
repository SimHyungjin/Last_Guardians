using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{
    public PlayerManager playerManager { get; private set; }
    public List<TowerData> TowerDatas { get; private set; }
    private int playerMaxHP = 5000;
    public int PlayerHP { get; private set; }
    public DamageText DamageTextPrefab { get; private set; }
    public Canvas DamageUICanvas { get; private set; }

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

    public int level;
    public int exp;
    private int maxExp = 1000;

    private void Awake()
    {
        InItTowerData();
        PlayerHP = playerMaxHP;
    }
    private void Start()
    {
        playerManager = new();
        playerManager.Init();
        PrefabInit();
        if(DamageUICanvas==null)
            DamageUICanvas = Instantiate(damageUICanvasPrefab);
        UpdateHP();
        UpdateExp();
        exp = 0;
        DateTime now = DateTime.Now;
        GameManager.Instance.NowTime = now.Minute;
        EnviromentManager.Instance.SetSeason(GameManager.Instance.NowTime);
        mulliganUI.StartSelectCard();
    }

    public void GetExp(int exp)
    {
        this.exp += exp;
        UpdateExp();
        if (this.exp >= maxExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Time.timeScale = 0f;
        level++;
        exp = exp - maxExp;
        UpdateExp();
        levelText.text = $"Lv {level}";
        TowerManager.Instance.StartInteraction(InteractionState.Pause);
        mulliganUI.gameObject.SetActive(true);
        mulliganUI.LevelUPSelect();
    }

    private void InItTowerData()
    {
        TowerDatas = Resources.LoadAll<TowerData>("SO/Tower").ToList();
        TowerDatas.Sort((a, b) => a.TowerIndex.CompareTo(b.TowerIndex));
    }

    public void GameStart()
    {
        MonsterManager.Instance.GameStart();
        GetExp(maxExp);
        //장애물 배치
    }

    public void AddCardTOHand(int index)
    {
        TowerManager.Instance.hand.AddCard(index);
        TowerManager.Instance.EndInteraction(InteractionState.Pause);
    }

    public void TakeDmage(int amount)
    {
        PlayerHP = Mathf.Max(0, PlayerHP - amount);
        UpdateHP();
        if (PlayerHP <= 0)
        {
            GameOver();
        }
            
    }

    private void PrefabInit()
    {
        DamageTextPrefab = Resources.Load<DamageText>("UI/DamageUI/DamageIndicator");
        damageUICanvasPrefab = Resources.Load<Canvas>("UI/DamageUI/DamageCanvas");
    }

    private void GameOver()
    {
        MonsterManager.Instance.StopAllCoroutines();
        TowerManager.Instance.StartInteraction(InteractionState.Pause);
        gameoverUI.gameObject.SetActive(true);
    }

    public void SetWaveInfoText(int wave,int count)
    {
        waveInfoText.text = $"{wave} Wave";
        remainMonsterCountText.text = $"다음 웨이브까지 남은 몬스터 수 {count}";
    }

    private void UpdateHP()
    {
        playerHPbar.fillAmount = (float)PlayerHP / playerMaxHP;
        
        playerHPText.text = $"현재 체력 : {PlayerHP} / {playerMaxHP}";
    }

    private void UpdateExp()
    {
        playerEXPbar.fillAmount = (float)exp / maxExp;
        playerEXPText.text = $"경험치 : {exp} / {maxExp}";
    }
}
