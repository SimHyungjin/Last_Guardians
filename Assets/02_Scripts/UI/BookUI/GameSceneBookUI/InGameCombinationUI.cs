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

        HashSet<int> selectedIdx = new HashSet<int>();
        if (selectedCards != null)
            foreach (var c in selectedCards)
                selectedIdx.Add(c.TowerIndex);

        var rules = combinationData.combinationRules
            .Where(r =>
                (r.ingredient1 == clicked.TowerIndex && selectedIdx.Contains(r.ingredient2)) ||
                (r.ingredient2 == clicked.TowerIndex && selectedIdx.Contains(r.ingredient1))
            )
            .ToList();

        foreach (var r in rules)
        {
            var a = TowerManager.Instance.GetTowerData(r.ingredient1);
            var b = TowerManager.Instance.GetTowerData(r.ingredient2);
            var res = TowerManager.Instance.GetTowerData(r.result);
            var go = Instantiate(cellPrefab, contentParent);
            var cell = go.GetComponent<CombinationCellUI>();
            if (cell == null)
            {
                Destroy(go);
                continue;
            }
            if (r.ingredient2 == clicked.TowerIndex)
                cell.Init(b, a, res);
            else
                cell.Init(a, b, res);
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
