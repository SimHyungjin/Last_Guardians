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
    [SerializeField] private List<Button> filterButtons;        // ���� ��ư ����: All, Fire, Water, Wind, Earth, Light, Dark, Standard, Mix
    [SerializeField] private Color defaultColor = Color.white;   // �⺻ ��ư ��
    [SerializeField] private Color selectedColor = Color.yellow; // ���õ� ��ư ��

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
        // ���� ��ư�� Ŭ�� ������ ���
        for (int i = 0; i < filterButtons.Count; i++)
        {
            int index = i;
            filterButtons[i].onClick.AddListener(() => OnFilterButtonClicked(index));
        }

        // �ʱ⿡�� "All" ���� ����
        if (filterButtons.Count > 0)
            OnFilterButtonClicked(0);
    }

    public void OnEnable()
    {
        // �� Ȱ��ȭ �� �� ������ �ڿ� ��ü ���ΰ�ħ
        StartCoroutine(DelayedRefresh());
    }

    private IEnumerator DelayedRefresh()
    {
        yield return null;
        RefreshToAll();
    }

    /// <summary>
    /// ��ư Ŭ�� �� ȣ��
    /// </summary>
    /// <param name="buttonIndex">��ư ����Ʈ �ε��� (0=All, 1=Fire, ...)</param>
    public void OnFilterButtonClicked(int buttonIndex)
    {
        // 1) ��ư �� �ʱ�ȭ
        foreach (var btn in filterButtons)
        {
            var img = btn.GetComponent<Image>();
            img.color = defaultColor;
        }

        // 2) Ŭ���� ��ư �� ����
        var clickedImg = filterButtons[buttonIndex].GetComponent<Image>();
        clickedImg.color = selectedColor;

        // 3) ���� ���� ����
        // buttonIndex - 1 ���� enum�� ���� ���
        SetFilter((TowerElementFilter)(buttonIndex - 1));
    }

    /// <summary>
    /// ��ü ������� ���ΰ�ħ
    /// </summary>
    public void RefreshToAll()
    {
        // "All" ��ư �ε����� 0
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
        // ���͸� ����
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

        // �������� ���� ������Ʈ 4�� �߰�
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
