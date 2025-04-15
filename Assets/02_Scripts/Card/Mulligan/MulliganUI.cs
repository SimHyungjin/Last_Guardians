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

    private int MaxSelectedCards = 2;
    private float timer = 30f;
    private int count = 0;
    private bool isTimerOn = false;

    public List<int> MyCardIndexList { get; private set; }
    public List<TowerData> MyCardList { get; private set; }

    private List<TowerData> elementalDataList;
    private List<TowerData> standardDataList;
    private List<MulliganCard> selectedCard;

    private void Awake()
    {
        selectedCard = new List<MulliganCard>();
        MyCardIndexList = new List<int>();
        MyCardList = new List<TowerData>();
        standardDataList = new List<TowerData>();

        elementalDataList = InGameManager.Instance.TowerDatas.FindAll(a => a.TowerType == TowerType.Elemental);
        standardDataList = InGameManager.Instance.TowerDatas.FindAll(a => a.TowerType == TowerType.Standard);

        Shuffle(elementalDataList);
        Shuffle(standardDataList);
    }

    private void Update()
    {
        if (isTimerOn)
        {
            UpdateTimer();
        }
        
    }

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

    public void StartSelectCard()
    {
        okBtn.onClick.AddListener(AddMyList);
        isTimerOn = true;
        ShowCardSelect(elementalDataList, cardNum);
    }

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

        Debug.Log($"현재 선택된 카드 수: {selectedCard.Count}");
    }

    public void AddMyList()
    {
        if (!IsValidSelection()) return;

        SaveSelectedCards();
        ClearUI();

        count++;
        timer = 30f;

        ProceedToNextStep();
    }

    private bool IsValidSelection()
    {
        if (selectedCard.Count != MaxSelectedCards)
        {
            Debug.Log($"카드 {MaxSelectedCards}개 선택하지 않음");
            return false;
        }
        return true;
    }

    private void SaveSelectedCards()
    {
        foreach (var card in selectedCard)
        {
            MyCardIndexList.Add(card.TowerIndex);
            MyCardList.Add(InGameManager.Instance.TowerDatas.Find(a => a.TowerIndex == card.TowerIndex));
            elementalDataList.RemoveAll(a => a.TowerIndex == card.TowerIndex);
        }
    }

    private void ClearUI()
    {
        DestroyAllChildren(parent);
        DestroyAllChildren(descriptionTransfrom);
        selectedCard.Clear();
    }

    private void ProceedToNextStep()
    {
        
        if (count <= 1)
        {
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

    private void EndMulligan()
    {
        MaxSelectedCards = 2;
        isTimerOn = false;
        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(AddCardtoHands);
        timerText.gameObject.SetActive(false);
        gameObject.SetActive(false);
        InGameManager.Instance.GameStart();
    }

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
            Debug.Log("자동 선택 카드가 부족");
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

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

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
        Shuffle(MyCardList);
        ShowCardSelect(MyCardList, 3);
        Time.timeScale = 0f;
    }

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
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        MaxSelectedCards = 1;
        remianCardNumText.text = "선택해야 하는 카드 수 : " + MaxSelectedCards;
    }
}
