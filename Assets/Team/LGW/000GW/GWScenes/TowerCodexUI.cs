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
    public GameObject entryPrefab;
    public Transform gridParent;
    public GameObject dummySpacerPrefab;
    public List<TowerData> towerDataList;

    private List<TowerData> allTowerData;
    private TowerElementFilter currentFilter = TowerElementFilter.All;

    private ScrollRect scrollRect;

    private void Awake()
    {
        allTowerData = Resources.LoadAll<TowerData>("SO/Tower").ToList();
        scrollRect = GetComponentInChildren<ScrollRect>();
    }

    private void OnEnable()
    {
        RefreshToAll();
    }

    public void OnFilterButtonClicked(int filterIndex)
    {
        SetFilter((TowerElementFilter)(filterIndex - 1));
    }

    public void RefreshToAll()
    {
        currentFilter = TowerElementFilter.All;
        FilterAndGenerate();  
    }


    private void SetFilter(TowerElementFilter filter)
    {
        currentFilter = filter;

        if (filter == TowerElementFilter.All)
        {
            towerDataList = allTowerData.OrderBy(t => t.TowerIndex).ToList();
        }
        else if (filter == TowerElementFilter.Standard)
        {
            towerDataList = allTowerData
                .Where(t => t.ElementType == ElementType.Standard)
                .OrderBy(t => t.TowerIndex).ToList();
        }
        else if (filter == TowerElementFilter.Mix)
        {
            towerDataList = allTowerData
                .Where(t => t.ElementType == ElementType.Steam ||
                            t.ElementType == ElementType.Lava ||
                            t.ElementType == ElementType.Storm ||
                            t.ElementType == ElementType.Swamp ||
                            t.ElementType == ElementType.Underground ||
                            t.ElementType == ElementType.LightDark)
                .OrderBy(t => t.TowerIndex).ToList();
        }
        else
        {
            towerDataList = allTowerData
                .Where(t => (int)t.ElementType == (int)filter)
                .OrderBy(t => t.TowerIndex).ToList();
        }

        GenerateCodex();
    }

    private void GenerateCodex()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        foreach (TowerData data in towerDataList)
        {
            GameObject entryGO = Instantiate(entryPrefab, gridParent);
            var entry = entryGO.GetComponent<TowerEntryUI>();
            entry.SetData(data);
        }

        if (dummySpacerPrefab != null)
        {
            Instantiate(dummySpacerPrefab, gridParent);
        }

        ResetScroll();
    }

    private void ResetScroll()
    {
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
    private void FilterAndGenerate()
    {
        if (currentFilter == TowerElementFilter.All)
        {
            towerDataList = allTowerData.OrderBy(t => t.TowerIndex).ToList();
        }
        else if (currentFilter == TowerElementFilter.Mix)
        {
            towerDataList = allTowerData
                .Where(t => !IsBasicElement(t.ElementType))
                .OrderBy(t => t.TowerIndex).ToList();
        }
        else
        {
            towerDataList = allTowerData
                .Where(t => (ElementType)(int)currentFilter == t.ElementType)
                .OrderBy(t => t.TowerIndex).ToList();
        }

        GenerateCodex();
    }
    private bool IsBasicElement(ElementType element)
    {
        return element == ElementType.Fire || element == ElementType.Water || element == ElementType.Wind ||
               element == ElementType.Earth || element == ElementType.Light || element == ElementType.Dark;
    }

}
