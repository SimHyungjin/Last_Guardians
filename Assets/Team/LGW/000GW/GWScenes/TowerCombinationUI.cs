using System.Linq;
using UnityEngine;

public class TowerCombinationUI : MonoBehaviour
{
    public static TowerCombinationUI Instance;

    public TowerCombinationData combinationData;
    public TowerSlot slot1, slot2, slot3;

    [HideInInspector] public TowerCodexUI codexUI; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowCombinationFor(TowerData resultData)
    {
        var rule = combinationData.combinationRules.FirstOrDefault(r => r.result == resultData.TowerIndex);
        if (rule == null)
        {
            slot1.SetData(null);
            slot2.SetData(null);
            slot3.SetData(resultData);
            return;
        }

        TowerData ing1 = TowerManager.Instance.GetTowerData(rule.ingredient1);
        TowerData ing2 = TowerManager.Instance.GetTowerData(rule.ingredient2);

        slot1.SetData(ing1);
        slot2.SetData(ing2);
        slot3.SetData(resultData);
    }

    public bool HasCombinationFor(TowerData data)
    {
        return combinationData.combinationRules.Any(r => r.result == data.TowerIndex);
    }

    public void CloseCombinationUI()
    {
        this.gameObject.SetActive(false);
        codexUI.UnlockCodexInteraction(); 
    }
}
