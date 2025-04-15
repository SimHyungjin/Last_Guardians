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

    private const int MaxSelectedCards = 2;
    private float timer = 30f;
    private int count = 0;
    private bool isTimerOn = false;

    public List<int> MyCardIndexList { get; private set; }
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

        Shuffle(elementalDataList);
        Shuffle(standardDataList);

        StartSelectCard();
    }

    private void Update()
    {
        if (!isTimerOn)
            return;

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
        timerText.text = Mathf.Round(timer).ToString();

        if (timer <= 0)
        {
            timer = 0;
            isTimerOn = false;
            AutoSelectCard();
        }
    }

    public void StartSelectCard()
    {
        ShowCardSelect(elementalDataList, cardNum);
    }

    private void ShowCardSelect(List<TowerData> dataList, int numberOfCards)
    {
        isTimerOn = true;

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
        if (selectedCard.Count != MaxSelectedCards)
        {
            Debug.Log("카드 2개 선택하지 않음");
            return;
        }

        count++;
        timer = 30f;

        foreach (var card in selectedCard)
        {
            MyCardIndexList.Add(card.TowerIndex);
            int index = elementalDataList.FindIndex(a => a.TowerIndex == card.TowerIndex);
            if (index != -1)
            {
                elementalDataList.RemoveAt(index);
            }
        }

        DestroyAllChildren(parent);
        DestroyAllChildren(descriptionTransfrom);
        selectedCard.Clear();
        Shuffle(elementalDataList);

        if (count <= 1)
        {
            ShowCardSelect(elementalDataList, cardNum);
        }
        else if (count == 2)
        {
            ShowCardSelect(standardDataList, cardNum + 1);
        }
        else
        {
            gameObject.SetActive(false);
            InGameManager.Instance.GameStart();
        }
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

        while (selectedCard.Count < MaxSelectedCards && availableCards.Count > 0)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            MulliganCard randomCard = availableCards[randomIndex];
            AddSelectCardList(randomCard);
            availableCards.RemoveAt(randomIndex);
        }

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
}
