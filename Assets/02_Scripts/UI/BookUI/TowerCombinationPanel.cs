using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerCombinationPanel : MonoBehaviour
{
    [SerializeField] private TowerCombinationData combinationData;
    [SerializeField] private TowerData[] allTowerData;

    [SerializeField] private TowerSlot slot1;
    [SerializeField] private TowerSlot slot2;
    [SerializeField] private TowerSlot resultSlot;

    public void ShowCombinationForTower(TowerData resultData)
    {
        foreach (var rule in combinationData.combinationRules)
        {
            if (rule.result == resultData.TowerIndex)
            {
                TowerData ingredient1 = FindTowerData(rule.ingredient1);
                TowerData ingredient2 = FindTowerData(rule.ingredient2);

                slot1.SetData(ingredient1 != null ? ingredient1 : null);
                slot2.SetData(ingredient2 != null ? ingredient2 : null);
                resultSlot.SetData(resultData);

                gameObject.SetActive(true);
                return;
            }
        }

        Debug.Log("해당 타워의 조합식이 존재하지 않음");
    }

    private TowerData FindTowerData(int index)
    {
        foreach (var data in allTowerData)
        {
            if (data.TowerIndex == index)
                return data;
        }
        return null;
    }
}
