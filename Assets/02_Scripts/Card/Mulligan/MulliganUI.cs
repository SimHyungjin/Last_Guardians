using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MulliganUI : MonoBehaviour
{
    [SerializeField] private MulliganCard CardPrefab;
    [SerializeField] private TextMeshProUGUI desTextPrefab;
    [SerializeField] private SpriteAtlas atlas;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform descriptionTransfrom;
    [SerializeField] private Button okBtn;
    [SerializeField] private int cardNum = 3;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI remianCardNumText;

    private int MaxSelectedCards = 2; //멀리건에서 선택해야 하는 카드 수
    private float timer = 30f; // 멀리건 타이머
    private int count = 0; // 멀리건에서 고른 카드 수
    private bool isTimerOn = false;

    public List<int> MyCardIndexList { get; private set; } //선택한 카드 인덱스
    public List<TowerData> MyCardList { get; private set; } // 선택한 카드 리스트

    private List<TowerData> elementalDataList; // 속성타워 카드리스트
    private List<TowerData> standardDataList; // 일반타워 카드시르트
    private List<MulliganCard> selectedCard; // 멀리건에서 고른 카드 리스트

    private void Awake()
    {
        selectedCard = new List<MulliganCard>();
        MyCardIndexList = new List<int>();
        MyCardList = new List<TowerData>();
        standardDataList = new List<TowerData>();

        elementalDataList = InGameManager.Instance.TowerDatas.FindAll(a => a.TowerType == TowerType.Elemental);
        standardDataList = InGameManager.Instance.TowerDatas.FindAll(a => a.TowerType == TowerType.Standard);

        //초기 카드 섞기

        Utils.Shuffle(elementalDataList);
        Utils.Shuffle(standardDataList);
    }

    private void Update()
    {
        if (isTimerOn)
        {
            UpdateTimer();
        }
        
    }

    //카드선택 남은시간 타이머
    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
        timerText.text = "남은 시간 : " + Mathf.Round(timer).ToString();

        if (timer <= 0)
        {
            timer = 0;
            //isTimerOn = false;
            AutoSelectCard();
        }
    }

    //카드선택 시작
    public void StartSelectCard()
    {
        okBtn.onClick.AddListener(AddMyList);
        isTimerOn = true;
        ShowCardSelect(elementalDataList, cardNum);
    }

    //선택할 카드 보여주기
    private void ShowCardSelect(List<TowerData> dataList, int numberOfCards)
    {
        remianCardNumText.text = "선택해야 하는 카드 수 : " + MaxSelectedCards;
        for (int i = 0; i < numberOfCards; i++)
        {
            MulliganCard card = Instantiate(CardPrefab, parent);
            TextMeshProUGUI des = Instantiate(desTextPrefab, descriptionTransfrom);
            card.Init(dataList[i].TowerIndex);
            des.text = dataList[i].TowerDescription;
            card.Btn.onClick.AddListener(() => AddSelectCardList(card));
        }
    }

    //카드 선택하기
    private void AddSelectCardList(MulliganCard card)
    {
        if (card.Outline.enabled)
        {
            selectedCard.Remove(card);
            card.Outline.enabled = false;
        }
        else
        {
            if (selectedCard.Count >= MaxSelectedCards)
            {
                selectedCard[0].Outline.enabled = false;
                selectedCard.RemoveAt(0);
            }
            selectedCard.Add(card);
            card.Outline.enabled = true;
        }
    }

    //선택한 카드를 덱에 넣기
    public void AddMyList()
    {
        if (!IsValidSelection()) return;

        SaveSelectedCards();
        ClearUI();

        count++;
        timer = 30f;

        ProceedToNextStep();
    }

    //선택된 카드가 골라야하는 만큼 골랐는지 확인
    private bool IsValidSelection()
    {
        if (selectedCard.Count != MaxSelectedCards)
        {
            return false;
        }
        return true;
    }

    //리스트에 저장
    private void SaveSelectedCards()
    {
        foreach (var card in selectedCard)
        {
            MyCardIndexList.Add(card.TowerIndex);
            MyCardList.Add(InGameManager.Instance.TowerDatas.Find(a => a.TowerIndex == card.TowerIndex));
            elementalDataList.RemoveAll(a => a.TowerIndex == card.TowerIndex);
        }
    }

    //UI 클리어
    private void ClearUI()
    {
        DestroyAllChildren(parent);
        DestroyAllChildren(descriptionTransfrom);
        selectedCard.Clear();
    }

    //멀리건UI 분기점
    private void ProceedToNextStep()
    {
        if (count <= 1)
        {
            Utils.Shuffle(elementalDataList);
            ShowCardSelect(elementalDataList, cardNum);
            MaxSelectedCards = 1;

        }
        else if (count == 2)
        {
            ShowCardSelect(standardDataList, cardNum + 1);
            MaxSelectedCards = 2;
        }
        else
        {
            EndMulligan();
        }

       
        remianCardNumText.text = "선택해야 하는 카드 수 : " + MaxSelectedCards;
    }

    //멀리건 종료
    private void EndMulligan()
    {
        MaxSelectedCards = 2;
        isTimerOn = false;
        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(AddCardtoHands);
        timerText.gameObject.SetActive(false);
        gameObject.SetActive(false);

        AnalyticsLogger.LogTowerSelect(MyCardList);

        InGameManager.Instance.GameStart();
        
    }
    
    //타이머 다 됐을때 카드 자동선택
    private void AutoSelectCard()
    {
        if (selectedCard.Count == MaxSelectedCards)
        {
            AddMyList();
            return;
        }

        List<MulliganCard> availableCards = new List<MulliganCard>();

        foreach (Transform child in parent)
        {
            MulliganCard card = child.GetComponent<MulliganCard>();
            if (card != null && !card.Outline.enabled)
            {
                availableCards.Add(card);
            }
        }

        if (availableCards.Count == 0)
        {
            Debug.Log("자동 선택 카드 부족");
            return;
        }

        while (selectedCard.Count < MaxSelectedCards && availableCards.Count > 0)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            MulliganCard randomCard = availableCards[randomIndex];
            AddSelectCardList(randomCard);
            availableCards.RemoveAt(randomIndex);
        }
        timer = 30f;
        AddMyList();
    }


    //UI객체 밑에 전부 파괴
    private void DestroyAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    //레벨업 시 호출
    public void LevelUPSelect()
    {
        Utils.Shuffle(MyCardList);
        ShowCardSelect(MyCardList, 3);
        Time.timeScale = 0f;
    }

    //선택한 카드 패로
    public void AddCardtoHands()
    {
        if (selectedCard.Count != MaxSelectedCards)
        {
            Debug.Log($"카드 {MaxSelectedCards}개 선택되지 않음");
            return;
        }

        foreach (var card in selectedCard)
        {
            InGameManager.Instance.AddCardTOHand(card.TowerIndex);
        }

        ClearUI();
        remianCardNumText.text = "선택해야 하는 카드 수 : " + MaxSelectedCards;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        MaxSelectedCards = 1;
        
        if (InGameManager.Instance.exp >= InGameManager.Instance.GetMaxExp())
        {
            InGameManager.Instance.LevelUp();
        }
    }
}
