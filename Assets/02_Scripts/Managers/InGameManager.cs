using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : Singleton<InGameManager>
{
    public PlayerManager playerManager { get; private set; }
    public List<TowerData> TowerDatas { get; private set; }
    public int PlayerHP { get; private set; } = 20;

    //[SerializeField] private DeckHandler deckHandler;
    [SerializeField] private MulliganUI mulliganUI;

    public int level;
    public int exp;
    public int maxExp;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InItTowerData();
    }
    private void Start()
    {
        playerManager = new();
        playerManager.Init();
        mulliganUI.StartSelectCard();
    }

    public void GetExp(int exp)
    {
        this.exp += exp;
        if (this.exp >= maxExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        level++;
        exp = 0;
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
        LevelUp();
    }

    public void AddCardTOHand(int index)
    {
        TowerManager.Instance.hand.AddCard(index);
        TowerManager.Instance.EndInteraction(InteractionState.Pause);
    }

    public void TakeDmage(int amount)
    {
        PlayerHP -= amount;
    }
}
