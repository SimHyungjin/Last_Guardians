using System.Linq;
using UnityEngine;

public class TowerCombinationUI : MonoBehaviour
{
    public static TowerCombinationUI Instance;

    public TowerCombinationData combinationData;
    public TowerSlot slot1, slot2, slot3;
    public GameObject tcSlotPanel;
    public GameObject fullscreenBlocker;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        tcSlotPanel.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    public void ShowCombinationFor(TowerData resultData)
    {
        var rule = combinationData.combinationRules.FirstOrDefault(r => r.result == resultData.TowerIndex);

        if (rule == null)
        {
            slot1.SetData(null);
            slot2.SetData(null);
            slot3.SetData(resultData);
        }
        else
        {
            TowerData ing1 = TowerManager.Instance.GetTowerData(rule.ingredient1);
            TowerData ing2 = TowerManager.Instance.GetTowerData(rule.ingredient2);

            slot1.SetData(ing1 != null ? ing1 : null);
            slot2.SetData(ing2 != null ? ing2 : null);
            slot3.SetData(resultData);
        }

        tcSlotPanel.SetActive(true);
        fullscreenBlocker.SetActive(true);
    }

    public void HidePanel()
    {
        tcSlotPanel.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    public bool HasCombinationFor(TowerData data)
    {
        return combinationData.combinationRules.Any(r => r.result == data.TowerIndex);
    }

    public void OnClickSlot(int slotIndex)
    {
        TowerSlot selected = slotIndex switch
        {
            0 => slot1,
            1 => slot2,
            2 => slot3,
            _ => null
        };

        if (selected?.Data == null) return;

        HidePanel();
        TowerCodexUI.Instance.ScrollToTower(selected.Data);
    }
}
