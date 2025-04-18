using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerCodexUI : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform gridParent;
    public List<TowerData> towerDataList;

    [Header("스크롤 & 버튼 컨트롤")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject dummySpacerPrefab;
    private List<Button> entryButtons = new List<Button>();

    void Start()
    {
        towerDataList = new List<TowerData>(Resources.LoadAll<TowerData>("SO/Tower"));
        towerDataList = towerDataList.OrderBy(t => t.TowerIndex).ToList();

        GenerateCodex();
    }

    public void GenerateCodex()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        entryButtons.Clear();

        foreach (TowerData data in towerDataList)
        {
            var entryGO = Instantiate(entryPrefab, gridParent);
            var entry = entryGO.GetComponent<TowerEntryUI>();
            entry.SetData(data);
            entryButtons.Add(entry.GetButton()); 
        }

        if (dummySpacerPrefab != null)
        {
            Instantiate(dummySpacerPrefab, gridParent);
        }
    }

    [ContextMenu("Find All TowerData")]
    public void FindAllTowerData()
    {
        towerDataList = Resources.LoadAll<TowerData>("SO/Tower").ToList();
    }

    
    public void LockCodexInteraction()
    {
        if (scrollRect != null) scrollRect.enabled = false;
        foreach (var btn in entryButtons)
        {
            if (btn != null) btn.interactable = false;
        }
    }

    public void UnlockCodexInteraction()
    {
        if (scrollRect != null) scrollRect.enabled = true;
        foreach (var btn in entryButtons)
        {
            if (btn != null) btn.interactable = true;
        }
    }
}
