// InGameTowerCodexUI.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTowerCodexUI : MonoBehaviour
{
    public static InGameTowerCodexUI Instance { get; private set; }
    [Header("멀리건 UI")]
    [SerializeField] private MulliganUI mulliganUI;

    [Header("Entry & Layout")]
    [SerializeField] private GameObject entryPrefab;        // InGameTowerEntryUI가 붙은 프리팹
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject dummySpacerPrefab;

    [Header("ScrollView")]
    [SerializeField] private ScrollRect scrollRect;

    private List<TowerData> towerDataList = new List<TowerData>();
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void HideCodexPanel()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        RefreshCodex();
    }

    public void ShowSingle(TowerData tower)
    {
      
        var list = new List<TowerData> { tower };

        
        list.Sort((a, b) => a.TowerIndex.CompareTo(b.TowerIndex));

      
        GenerateCodex(list);

        
        gameObject.SetActive(true);
    }

    public void RefreshCodex()
    {
        var myTowers = mulliganUI.MyCardList;
        if (myTowers == null || myTowers.Count == 0)
        {
            ClearGrid();
            return;
        }

        towerDataList = new List<TowerData>(myTowers);
        towerDataList.Sort((a, b) => a.TowerIndex.CompareTo(b.TowerIndex));

        GenerateCodex(towerDataList);
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
            var entryUI = go.GetComponent<InGameTowerEntryUI>();
            entryUI.Init(data);
        }

        for (int i = 0; i < 4; i++)
            Instantiate(dummySpacerPrefab, gridParent);

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
