using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TowerCombinationUI : MonoBehaviour
{
    public static TowerCombinationUI Instance;

    [Header("���� ������")]
    public TowerCombinationData combinationData;

    [Header("UI ����")]
    public TowerSlot slot1, slot2, slot3;
    public GameObject tcSlotPanel; // ���Ե��� ���Ե� Panel
    public GameObject fullscreenBlocker; // Ŭ�� ������ ���� �г�
    

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
            Debug.LogWarning("���ս� ����");
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

        // ��ü ���� UI �Ѵ� ���
        tcSlotPanel.SetActive(true); // �� �̰Ÿ� ON

        // ��� Ŭ������ �ݴ� ��쵵 ����ؼ�
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
