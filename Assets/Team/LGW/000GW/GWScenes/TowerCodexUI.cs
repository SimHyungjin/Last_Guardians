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

    void Start()
    {
        // 전체 타워 데이터 초기화
        allTowerData = Resources.LoadAll<TowerData>("SO/Tower").ToList();
        towerDataList = allTowerData.OrderBy(t => t.TowerIndex).ToList();

        GenerateCodex();
    }

    public void OnFilterButtonClicked(int filterIndex)
    {
        currentFilter = (TowerElementFilter)(filterIndex - 1);
        FilterAndGenerate();
    }


    private void FilterAndGenerate()
    {
        if (currentFilter == TowerElementFilter.All)
        {
            towerDataList = allTowerData;
        }
        else if (currentFilter == TowerElementFilter.Standard)
        {
            towerDataList = allTowerData.Where(t => t.ElementType == ElementType.Standard).ToList();
        }
        else if (currentFilter == TowerElementFilter.Mix)
        {
            towerDataList = allTowerData.Where(t =>
                t.ElementType == ElementType.Steam ||
                t.ElementType == ElementType.Lava ||
                t.ElementType == ElementType.Storm ||
                t.ElementType == ElementType.Swamp ||
                t.ElementType == ElementType.Underground ||
                t.ElementType == ElementType.LightDark
            ).ToList();
        }
        else
        {
            // Fire ~ Dark
            towerDataList = allTowerData.Where(t => (ElementType)(int)currentFilter == t.ElementType).ToList();
        }

        GenerateCodex();
    }

    private void GenerateCodex()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        foreach (var data in towerDataList)
        {
            var entryGO = Instantiate(entryPrefab, gridParent);
            var entry = entryGO.GetComponent<TowerEntryUI>();
            entry.SetData(data);
        }

        if (dummySpacerPrefab != null)
        {
            Instantiate(dummySpacerPrefab, gridParent);
        }
    }
}
