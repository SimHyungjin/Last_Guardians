using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum TowerElementFilter
{
    All = -1, Fire = 0, Water, Wind, Earth, Light, Dark, Standard, Mix
}

public class TowerCodexUI : MonoBehaviour
{
    public static TowerCodexUI Instance;

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

        allTowerData = Resources.LoadAll<TowerData>("SO/Tower").ToList();
        if (entryPrefab == null) entryPrefab = Resources.Load<GameObject>("UI/MainScene/TowerEntry");
        if (dummySpacerPrefab == null) dummySpacerPrefab = Resources.Load<GameObject>("UI/MainScene/TowerDummySpacer");
    }

    private void OnEnable()
    {
        StartCoroutine(DelayedRefresh());
    }

    private IEnumerator DelayedRefresh()
    {
        yield return null;
        RefreshToAll();
    }

    public void OnFilterButtonClicked(int filterIndex)
    {
        SetFilter((TowerElementFilter)(filterIndex - 1));
    }

    public void RefreshToAll()
    {
        SetFilter(TowerElementFilter.All);
    }

    private void SetFilter(TowerElementFilter filter)
    {
        currentFilter = filter;
        FilterAndGenerate();
    }

    private void FilterAndGenerate()
    {
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

        if (dummySpacerPrefab != null)
            Instantiate(dummySpacerPrefab, gridParent);

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private bool IsBasicElement(ElementType e)
        => e == ElementType.Fire || e == ElementType.Water || e == ElementType.Wind
        || e == ElementType.Earth || e == ElementType.Light || e == ElementType.Dark;

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
