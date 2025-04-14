using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MulliganUI : MonoBehaviour
{
    [SerializeField] MulliganCard CradPrefab;
    [SerializeField] private SpriteAtlas atlas;
    private List<TowerData> dataList;
    private List<int> randomIDs = new List<int>();
    private int CardNum = 3;

    private void Start()
    {
        dataList = InGameManager.Instance.TowerDatas.FindAll(a => a.TowerType == TowerType.Elemental);
        PickRandomTower();

        for(int i =0; i<CardNum; i++)
        {
            MulliganCard card = Instantiate(CradPrefab);
            card.Init(randomIDs[i]);
        }        
    }

    private void PickRandomTower()
    {
        for(int i = 0; i < CardNum; i++)
        {
            int randomId = Random.Range(0, dataList.Count);
            randomIDs.Add(dataList[randomId].TowerIndex);
        }
    }
}
