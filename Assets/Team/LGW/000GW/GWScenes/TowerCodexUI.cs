using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TowerCodexUI : MonoBehaviour
{
    public GameObject entryPrefab; 
    public Transform entryParent;
    public List<TowerData> towerDataList;


    void Start()
    {
        towerDataList = new List<TowerData>(Resources.LoadAll<TowerData>("SO/Tower"));

        
        towerDataList = towerDataList.OrderBy(t => t.TowerIndex).ToList();

        GenerateCodex();
    }



    void GenerateCodex()
    {
        Debug.Log($"타워 개수: {towerDataList.Count}");

        foreach (Transform child in entryParent)
            Destroy(child.gameObject);

        foreach (TowerData data in towerDataList)
        {
            Debug.Log($"타워 생성 중: {data.TowerName}");

            var entryGO = Instantiate(entryPrefab, entryParent);
            var entry = entryGO.GetComponent<TowerEntryUI>();
            entry.SetData(data);
        }
    }
    [ContextMenu("Find All TowerData")]
    public void FindAllTowerData()
    {
        towerDataList = Resources.LoadAll<TowerData>("SO/Tower").ToList();
    }
}

