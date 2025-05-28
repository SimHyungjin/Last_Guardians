using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InGameCombinationUI : MonoBehaviour
{
    public static InGameCombinationUI Instance { get; private set; }

    [SerializeField] private TowerCombinationData combinationData;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject dummySpacerPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject panelContainer;
    [SerializeField] private GameObject fullscreenBlocker;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        panelContainer.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    public void ShowFor(TowerData clicked)
    {
        List<TowerData> selected = MulliganUI.Instance != null
            ? MulliganUI.Instance.GetSelectedCards()
            : new List<TowerData>();
        ShowFor(clicked, selected);
    }

    public void ShowFor(TowerData clicked, List<TowerData> selectedCards)
    {
        for (int i = contentParent.childCount - 1; i >= 0; i--)
            Destroy(contentParent.GetChild(i).gameObject);

        var selectedIdx = selectedCards?
            .Select(c => c.TowerIndex)
            .ToHashSet()
            ?? new HashSet<int>();
        if (!selectedIdx.Contains(clicked.TowerIndex))
            selectedIdx.Add(clicked.TowerIndex);
             
        var available = new HashSet<int>(selectedIdx);
              
        var reachable = new Dictionary<int, bool>();
        foreach (var idx in available)
            reachable[idx] = (idx == clicked.TowerIndex);

        var frontier = new Queue<int>(available);
        var foundRules = new List<TowerCombinationRule>();

        while (frontier.Count > 0)
        {
            int curr = frontier.Dequeue();

            foreach (var rule in combinationData.combinationRules)
            {                
                if (rule.ingredient1 != curr && rule.ingredient2 != curr)
                    continue;
                               
                int other = rule.ingredient1 == curr
                    ? rule.ingredient2
                    : rule.ingredient1;
                if (!available.Contains(other))
                    continue;
                               
                if (available.Add(rule.result))
                {
                    frontier.Enqueue(rule.result);
                    foundRules.Add(rule);


                    bool isReach =
                        reachable.TryGetValue(rule.ingredient1, out var r1) && r1
                     || reachable.TryGetValue(rule.ingredient2, out var r2) && r2;
                    reachable[rule.result] = isReach;
                }
            }
        }

        foreach (var r in foundRules)
        {
            if (!reachable.TryGetValue(r.result, out var ok) || !ok)
                continue;

            
            var data1 = TowerManager.Instance.GetTowerData(r.ingredient1);
            var data2 = TowerManager.Instance.GetTowerData(r.ingredient2);
            var dataRes = TowerManager.Instance.GetTowerData(r.result);

            bool firstIsClicked;
            if (r.ingredient1 == clicked.TowerIndex)
                firstIsClicked = true;
            else if (r.ingredient2 == clicked.TowerIndex)
                firstIsClicked = false;
            else               
                firstIsClicked = true;

            
            var go = Instantiate(cellPrefab, contentParent);
            var cell = go.GetComponent<CombinationCellUI>();
            if (cell != null)
            {
                if (firstIsClicked)
                    cell.Init(data1, data2, dataRes);
                else
                    cell.Init(data2, data1, dataRes);
            }
        }
        Instantiate(dummySpacerPrefab, contentParent);
        panelContainer.SetActive(true);
        fullscreenBlocker.SetActive(true);
        scrollRect.verticalNormalizedPosition = 1f;
    }





    public void HideAndReset()
    {
        for (int i = contentParent.childCount - 1; i >= 0; i--)
            Destroy(contentParent.GetChild(i).gameObject);
        panelContainer.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    public void Hide() => HideAndReset();
}
