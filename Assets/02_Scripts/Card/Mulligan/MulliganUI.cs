using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MulliganUI : MonoBehaviour
{
    [SerializeField] private MulliganCard CradPrefab;
    [SerializeField] private SpriteAtlas atlas;
    [SerializeField] private Transform parent;
    private List<TowerData> elementalDataList;
    private List<MulliganCard> selectedCard;
    private int CardNum = 3;

    private void Start()
    {
        elementalDataList = InGameManager.Instance.TowerDatas.FindAll(a => a.TowerType == TowerType.Elemental);
        Shuffle<TowerData>(elementalDataList);
        ShowSelectCard();
    }

    
    //랜덤섞기
    public void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count); // i 이상 list.Count 미만
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private void ShowSelectCard()
    {
        for (int i = 0; i < CardNum; i++)
        {
            MulliganCard card = Instantiate(CradPrefab, parent);
            card.Init(elementalDataList[i].TowerIndex);
        }
    }

}
