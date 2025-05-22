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
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        panelContainer.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    public void ShowFor(TowerData clicked)
    {
        ClearContent();

        var rules = combinationData.combinationRules
            .Where(r => r.ingredient1 == clicked.TowerIndex || r.ingredient2 == clicked.TowerIndex)
            .ToList();

        foreach (var r in rules)
        {
            var a = TowerManager.Instance.GetTowerData(r.ingredient1);
            var b = TowerManager.Instance.GetTowerData(r.ingredient2);
            var res = TowerManager.Instance.GetTowerData(r.result);

            var cell = Instantiate(cellPrefab, contentParent)
                .GetComponent<CombinationCellUI>();

            if (r.ingredient2 == clicked.TowerIndex) cell.Init(b, a, res);
            else cell.Init(a, b, res);
        }

        
        for (int i = 0; i < 1; i++)
            Instantiate(dummySpacerPrefab, contentParent);

        panelContainer.SetActive(true);
        fullscreenBlocker.SetActive(true);
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void HideAndReset()
    {
        ClearContent();
        panelContainer.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    public void Hide() => HideAndReset();

    private void ClearContent()
    {
        foreach (Transform t in contentParent)
            Destroy(t.gameObject);
    }
}
