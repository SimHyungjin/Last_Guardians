using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum TowerElementFilter
{
    All = -1,
    Fire = 0,
    Water,
    Wind,
    Earth,
    Light,
    Dark,
    Standard,
    Mix
}

public class TowerCodexUI : MonoBehaviour
{
    public static TowerCodexUI Instance;

    [Header("UI References")]
    [SerializeField] private List<Button> filterButtons;        // 필터 버튼 순서: All, Fire, Water, Wind, Earth, Light, Dark, Standard, Mix
    [SerializeField] private Color defaultColor = Color.white;   // 기본 버튼 색
    [SerializeField] private Color selectedColor = Color.yellow; // 선택된 버튼 색

    [Header("Codex Content")]
    public GameObject entryPrefab;
    public Transform gridParent;
    public GameObject dummySpacerPrefab;
    public ScrollRect scrollRect;

    private List<TowerData> allTowerData;
    private List<TowerData> towerDataList;
    private TowerElementFilter currentFilter = TowerElementFilter.All;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Load all TowerData SOs
        allTowerData = Resources.LoadAll<TowerData>("SO/Tower").ToList();
        if (entryPrefab == null) entryPrefab = Resources.Load<GameObject>("UI/MainScene/TowerEntry");
        if (dummySpacerPrefab == null) dummySpacerPrefab = Resources.Load<GameObject>("UI/MainScene/TowerDummySpacer");
    }

    private void Start()
    {
        // 필터 버튼에 클릭 리스너 등록
        for (int i = 0; i < filterButtons.Count; i++)
        {
            int index = i;
            filterButtons[i].onClick.AddListener(() => OnFilterButtonClicked(index));
        }

        // 초기에는 "All" 필터 선택
        if (filterButtons.Count > 0)
            OnFilterButtonClicked(0);
    }

    public void OnEnable()
    {
        // 씬 활성화 시 한 프레임 뒤에 전체 새로고침
        StartCoroutine(DelayedRefresh());
    }

    private IEnumerator DelayedRefresh()
    {
        yield return null;
        RefreshToAll();
    }

    /// <summary>
    /// 버튼 클릭 시 호출
    /// </summary>
    /// <param name="buttonIndex">버튼 리스트 인덱스 (0=All, 1=Fire, ...)</param>
    public void OnFilterButtonClicked(int buttonIndex)
    {
        // 1) 버튼 색 초기화
        foreach (var btn in filterButtons)
        {
            var img = btn.GetComponent<Image>();
            img.color = defaultColor;
        }

        // 2) 클릭된 버튼 색 변경
        var clickedImg = filterButtons[buttonIndex].GetComponent<Image>();
        clickedImg.color = selectedColor;

        // 3) 실제 필터 적용
        // buttonIndex - 1 값을 enum에 맞춰 사용
        SetFilter((TowerElementFilter)(buttonIndex - 1));
    }

    /// <summary>
    /// 전체 목록으로 새로고침
    /// </summary>
    public void RefreshToAll()
    {
        // "All" 버튼 인덱스는 0
        if (filterButtons.Count > 0)
            OnFilterButtonClicked(0);
    }

    private void SetFilter(TowerElementFilter filter)
    {
        currentFilter = filter;
        FilterAndGenerate();
    }

    private void FilterAndGenerate()
    {
        // 필터링 로직
        towerDataList = currentFilter switch
        {
            TowerElementFilter.All => allTowerData,
            TowerElementFilter.Standard => allTowerData.Where(t => t.ElementType == ElementType.Standard).ToList(),
            TowerElementFilter.Mix => allTowerData.Where(t => !IsBasicElement(t.ElementType)).ToList(),
            _ => allTowerData.Where(t => (int)t.ElementType == (int)currentFilter).ToList()
        };

        towerDataList = towerDataList.OrderBy(t => t.TowerIndex).ToList();
        GenerateCodex(towerDataList);
    }

    private void GenerateCodex(List<TowerData> list)
    {
        Canvas.ForceUpdateCanvases();
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        foreach (var data in list)
        {
            var go = Instantiate(entryPrefab, gridParent);
            var entry = go.GetComponent<TowerEntryUI>();
            entry.SetData(data);
        }

        // 마지막에 더미 오브젝트 4개 추가
        for (int i = 0; i < 4; i++)
            Instantiate(dummySpacerPrefab, gridParent);

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private bool IsBasicElement(ElementType e)
        => e == ElementType.Fire
        || e == ElementType.Water
        || e == ElementType.Wind
        || e == ElementType.Earth
        || e == ElementType.Light
        || e == ElementType.Dark;

    public void ScrollToTower(TowerData data)
    {
        StartCoroutine(ScrollAfterRebuild(data));
    }

    private IEnumerator ScrollAfterRebuild(TowerData data)
    {
        SetFilter(TowerElementFilter.All);
        yield return null;
        int index = towerDataList.FindIndex(t => t == data);
        if (index < 0) yield break;
        yield return new WaitForEndOfFrame();
        float normalized = 1f - (index / (float)(towerDataList.Count - 1));
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(normalized);
    }
}
