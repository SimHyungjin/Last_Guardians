using System.Linq;
using UnityEngine;

public class TowerCombinationUI : MonoBehaviour
{
    public static TowerCombinationUI Instance;

    [Header("조합 데이터")]
    public TowerCombinationData combinationData;

    [Header("조합 슬롯")]
    public TowerSlot slot1, slot2, slot3;

    [Header("UI 패널")]
    public GameObject tcSlotPanel;
    public GameObject fullscreenBlocker;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        tcSlotPanel.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    /// <summary>
    /// 지정한 타워의 조합식을 보여줍니다.
    /// </summary>
    public void ShowCombinationFor(TowerData resultData)
    {
        var rule = combinationData.combinationRules
                              .FirstOrDefault(r => r.result == resultData.TowerIndex);

        if (rule == null)
        {
            slot1.SetData(null);
            slot2.SetData(null);
            slot3.SetData(resultData);
        }
        else
        {
            var ing1 = TowerManager.Instance.GetTowerData(rule.ingredient1);
            var ing2 = TowerManager.Instance.GetTowerData(rule.ingredient2);

            slot1.SetData(ing1);
            slot2.SetData(ing2);
            slot3.SetData(resultData);
        }

        tcSlotPanel.SetActive(true);
        fullscreenBlocker.SetActive(true);
    }

    /// <summary>
    /// 조합식 패널을 닫습니다.
    /// </summary>
    public void HidePanel()
    {
        tcSlotPanel.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    /// <summary>
    /// 해당 타워에 조합식이 있는지 확인합니다.
    /// </summary>
    public bool HasCombinationFor(TowerData data)
        => combinationData.combinationRules
                          .Any(r => r.result == data.TowerIndex);

    /// <summary>
    /// 슬롯 클릭 시 호출합니다.
    /// 메인 씬 도감이 활성화된 경우에만 ScrollToTower 호출.
    /// </summary>
    public void OnClickSlot(int slotIndex)
    {
        TowerSlot selected = slotIndex switch
        {
            0 => slot1,
            1 => slot2,
            2 => slot3,
            _ => null
        };

        if (selected?.Data == null)
            return;

        // 조합식 닫기
        HidePanel();

        // 메인 씬 도감(TowerCodexUI)이 활성화되어 있을 때만 스크롤
        var mainCodex = TowerCodexUI.Instance;
        if (mainCodex != null && mainCodex.gameObject.activeInHierarchy)
        {
            mainCodex.ScrollToTower(selected.Data);
        }
    }
}
