using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum InTowerElementFilter
{
    All = 0, Fire, Water, Wind, Earth, Light, Dark, Standard, Mix
}

public class InGameTowerCodexUI : MonoBehaviour
{
    [Header("멀리건 UI (Inspector에서 연결)")]
    [SerializeField] private MulliganUI mulliganUI;

    [Header("조합 데이터 (합성 가능한 결과 타워)")]
    [SerializeField] private TowerCombinationData combinationData;

    [Header("Entry & Layout 설정")]
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject dummySpacerPrefab;

    [Header("Scroll View")]
    [SerializeField] private ScrollRect scrollRect;

    private InTowerElementFilter currentFilter = InTowerElementFilter.All;

    private void OnEnable()
    {
        // 한 프레임 대기 후에 갱신
        StartCoroutine(DelayedRefresh());
    }

    private IEnumerator DelayedRefresh()
    {
        yield return null;
        RefreshCodex();
    }

    public void OnFilterButtonClicked(int filterIndex)
    {
        currentFilter = (InTowerElementFilter)filterIndex;
        RefreshCodex();
    }

    public void RefreshCodex()
    {
        if (mulliganUI == null) return;

        var myTowers = mulliganUI.MyCardList;
        if (myTowers == null || myTowers.Count == 0)
        {
            ClearGrid();
            return;
        }

        // 1) 내가 고른 카드 필터링
        var filtered = ApplyFilter(myTowers);

        // 2) 합성 결과 추가
        if (combinationData != null)
        {
            var combos = GetComboResults(myTowers);
            foreach (var combo in combos)
                if (!filtered.Any(t => t.TowerIndex == combo.TowerIndex))
                    filtered.Add(combo);
        }

        // 3) 정렬 & 생성
        filtered.Sort((a, b) => a.TowerIndex.CompareTo(b.TowerIndex));
        GenerateCodex(filtered);
    }

    private List<TowerData> ApplyFilter(List<TowerData> source)
    {
        return currentFilter switch
        {
            InTowerElementFilter.All => new List<TowerData>(source),
            InTowerElementFilter.Standard => source.Where(t => t.ElementType == ElementType.Standard).ToList(),
            InTowerElementFilter.Mix => source.Where(t => t.ElementType != ElementType.Standard
                                                         && !IsBasicElement(t.ElementType)).ToList(),
            _ => source.Where(t => (int)t.ElementType == (int)currentFilter).ToList(),
        };
    }

    private bool IsBasicElement(ElementType e)
        => e == ElementType.Fire
        || e == ElementType.Water
        || e == ElementType.Wind
        || e == ElementType.Earth
        || e == ElementType.Light
        || e == ElementType.Dark;

    private List<TowerData> GetComboResults(List<TowerData> chosen)
    {
        var idxSet = new HashSet<int>(chosen.Select(t => t.TowerIndex));
        var results = new List<TowerData>();
        foreach (var rule in combinationData.combinationRules)
        {
            if (idxSet.Contains(rule.ingredient1) && idxSet.Contains(rule.ingredient2))
            {
                var data = TowerManager.Instance.GetTowerData(rule.result);
                if (data != null) results.Add(data);
            }
        }
        return results;
    }

    private void ClearGrid()
    {
        foreach (Transform c in gridParent)
            Destroy(c.gameObject);
    }

    private void GenerateCodex(List<TowerData> list)
    {
        Canvas.ForceUpdateCanvases();
        ClearGrid();

        foreach (var data in list)
        {
            var go = Instantiate(entryPrefab, gridParent);
            go.GetComponent<TowerEntryUI>().SetData(data);
        }

        for (int i = 0; i < 4; i++)
            Instantiate(dummySpacerPrefab, gridParent);

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
