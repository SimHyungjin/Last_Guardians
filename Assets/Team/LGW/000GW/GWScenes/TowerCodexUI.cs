using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerCodexUI : MonoBehaviour
{
    public GameObject entryPrefab;

   
    public Transform gridParent;

    public List<TowerData> towerDataList;

    void Start()
    {
        towerDataList = new List<TowerData>(Resources.LoadAll<TowerData>("SO/Tower"));
        towerDataList = towerDataList.OrderBy(t => t.TowerIndex).ToList();

        GenerateCodex();
    }

    [SerializeField] private GameObject dummySpacerPrefab;

    void GenerateCodex()
    {
        Debug.Log($"타워 개수: {towerDataList.Count}");

        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        foreach (TowerData data in towerDataList)
        {
            Debug.Log($"타워 생성 중: {data.TowerName}");

            var entryGO = Instantiate(entryPrefab, gridParent);
            var entry = entryGO.GetComponent<TowerEntryUI>();
            entry.SetData(data);
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
}
