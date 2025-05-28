using System.Linq;
using UnityEngine;

public class TowerCombinationUI : MonoBehaviour
{
    public static TowerCombinationUI Instance;

    [Header("���� ������")]
    public TowerCombinationData combinationData;

    [Header("���� ����")]
    public TowerSlot slot1, slot2, slot3;

    [Header("UI �г�")]
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
    /// ������ Ÿ���� ���ս��� �����ݴϴ�.
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
    /// ���ս� �г��� �ݽ��ϴ�.
    /// </summary>
    public void HidePanel()
    {
        tcSlotPanel.SetActive(false);
        fullscreenBlocker.SetActive(false);
    }

    /// <summary>
    /// �ش� Ÿ���� ���ս��� �ִ��� Ȯ���մϴ�.
    /// </summary>
    public bool HasCombinationFor(TowerData data)
        => combinationData.combinationRules
                          .Any(r => r.result == data.TowerIndex);

    /// <summary>
    /// ���� Ŭ�� �� ȣ���մϴ�.
    /// ���� �� ������ Ȱ��ȭ�� ��쿡�� ScrollToTower ȣ��.
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

        // ���ս� �ݱ�
        HidePanel();

        // ���� �� ����(TowerCodexUI)�� Ȱ��ȭ�Ǿ� ���� ���� ��ũ��
        var mainCodex = TowerCodexUI.Instance;
        if (mainCodex != null && mainCodex.gameObject.activeInHierarchy)
        {
            mainCodex.ScrollToTower(selected.Data);
        }
    }
}
