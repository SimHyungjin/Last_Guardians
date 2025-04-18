using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerCombinationUI : MonoBehaviour
{
    public static TowerCombinationUI Instance;

    [Header("조합 데이터")]
    public TowerCombinationData combinationData;

    [Header("UI 연결")]
    public TowerSlot slot1, slot2, slot3;
    public GameObject tcSlotPanel; // 슬롯들이 포함된 Panel
    public GameObject fullscreenBlocker; // 클릭 감지용 투명 패널
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        tcSlotPanel.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    

    public void ShowCombinationFor(TowerData resultData)
    {
        var rule = combinationData.combinationRules
            .FirstOrDefault(r => r.result == resultData.TowerIndex);

        if (rule == null)
        {
            Debug.LogWarning("조합식 없음");
            slot1.SetData(null);
            slot2.SetData(null);
            slot3.SetData(resultData);
        }
        else
        {
            TowerData ing1 = TowerManager.Instance.GetTowerData(rule.ingredient1);
            TowerData ing2 = TowerManager.Instance.GetTowerData(rule.ingredient2);

            slot1.SetData(ing1);
            slot2.SetData(ing2);
            slot3.SetData(resultData);
        }

        // 전체 조합 UI 켜는 대신
        tcSlotPanel.SetActive(true); // ← 이거만 ON

        // 배경 클릭으로 닫는 경우도 대비해서
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
}
