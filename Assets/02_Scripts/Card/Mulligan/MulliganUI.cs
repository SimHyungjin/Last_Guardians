using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MulliganUI : MonoBehaviour
{
    [SerializeField] private MulliganCard CardPrefab;
    [SerializeField] private SpriteAtlas atlas;
    [SerializeField] private Transform parent;
    [SerializeField] private Button okBtn;
    [SerializeField] private int cardNum = 3;
    [SerializeField] TextMeshProUGUI timerText;
    private float timer = 30f;
    private int count = 0;
    private bool isTimerOn = false;
    public List<int> MyCardIndexList { get; set; }
    private List<TowerData> elementalDataList;
    private List<TowerData> standardDataList;
    private List<MulliganCard> selectedCard;


    private void Awake()
    {
        selectedCard = new List<MulliganCard>();
        standardDataList = new List<TowerData>();
        MyCardIndexList = new List<int>();
    }

    private void Start()
    {
        elementalDataList = InGameManager.Instance.TowerDatas.FindAll(a => a.TowerType == TowerType.Elemental);
        standardDataList = InGameManager.Instance.TowerDatas.FindAll(a => a.TowerType == TowerType.Standard);
        okBtn.onClick.AddListener(AddMyList);
        Shuffle<TowerData>(elementalDataList);
        Shuffle<TowerData>(standardDataList);
        StartSelectCard();
    }

    private void Update()
    {
        if (isTimerOn)
            timer = Time.deltaTime;
    }

    //랜덤섞기
    public void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count); // i 이상 list.Count 미만
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    public void StartSelectCard()
    {
        ShowElenentalCardSelect();
    }

    private void ShowElenentalCardSelect()
    {
        isTimerOn = true;

        for (int i = 0; i < cardNum; i++)
        {
            MulliganCard card = Instantiate(CardPrefab, parent);
            card.Init(elementalDataList[i].TowerIndex);
            card.Btn.onClick.AddListener(() =>
            {
                AddSelectCardList(card);
            });
        }
    }

    private void AddSelectCardList(MulliganCard card)
    {
        if (card.Outline.enabled)
        {
            // 이미 선택된 카드면 해제
            selectedCard.Remove(card);
            card.Outline.enabled = false;
        }
        else
        {
            if (selectedCard.Count >= 2)
            {
                // 2개 넘으면 제일 오래된 선택 해제
                selectedCard[0].Outline.enabled = false;
                selectedCard.RemoveAt(0);
            }
            // 새로운 카드 선택
            selectedCard.Add(card);
            card.Outline.enabled = true;
        }

        Debug.Log(selectedCard.Count);

    }

    public void AddMyList()
    {
        if (selectedCard.Count != 2)
        {
            Debug.Log("2개를 선택하지 않음");
            return;
        }

        count++;

        for (int i = 0; i < selectedCard.Count; i++)
        {
            MyCardIndexList.Add(selectedCard[i].TowerIndex);
            int index = elementalDataList.FindIndex(a => a.TowerIndex == selectedCard[i].TowerIndex);
            if (index != -1)
            {
                elementalDataList.RemoveAt(index);
            }
        }
        DestroyAllChildren(parent.gameObject);
        selectedCard.Clear();
        Shuffle<TowerData>(elementalDataList);

        if (count <= 1)
            ShowElenentalCardSelect();
        else if(count == 2)
        {
            ShowStandardCardSelect();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void ShowStandardCardSelect()
    {
        for (int i = 0; i < cardNum+1; i++)
        {
            MulliganCard card = Instantiate(CardPrefab, parent);
            card.Init(standardDataList[i].TowerIndex);
            card.Btn.onClick.AddListener(() =>
            {
                AddSelectCardList(card);
            });
        }
    }

    void DestroyAllChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
