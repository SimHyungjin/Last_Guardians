using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePopup : MonoBehaviour
{
    [SerializeField] private Slot currentSlot;
    [SerializeField] private Slot upgradeSlot;

    [SerializeField] private TextMeshProUGUI upgradeResultText;

    [SerializeField] private TextMeshProUGUI attackPower;
    [SerializeField] private TextMeshProUGUI attackSpeed;
    [SerializeField] private TextMeshProUGUI attackRange;
    [SerializeField] private TextMeshProUGUI criticalChance;
    [SerializeField] private TextMeshProUGUI criticalDamage;
    [SerializeField] private TextMeshProUGUI penetration;
    [SerializeField] private TextMeshProUGUI moveSpeed;

    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI stone;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cancelButton;

    ItemInstance currectData;
    ItemInstance upgradeData;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClick);
        cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void Init(ItemInstance instance)
    {
        currectData = instance;
        currentSlot.SetData(currectData);
        upgradeData = GameManager.Instance.ItemManager.GetItemInstanceByIndex(instance.AsEquipData.ItemIndex + 100);
        upgradeSlot.SetData(upgradeData);
        RefreshText();
    }

    private void RefreshText()
    {
        upgradeButton.interactable = true;
        var upgradeSystem = MainSceneManager.Instance.upgrade;
        var rule = upgradeSystem.GetUpgradeRules().FirstOrDefault(x => x.sourceGrade == currectData.Data.itemGrade);

        if (rule == null)
        {
            upgradeResultText.text = "최종 상태";
            upgradeButton.interactable = false;
            return;
        }

        upgradeResultText.text = $"{currectData.Data.itemGrade} → {upgradeData.Data.itemGrade}\n성공확률 : {rule.successRate}%";

        EquipData before = currectData.AsEquipData;
        EquipData after = upgradeData.AsEquipData;

        SetStatText(attackPower, before.attackPower, after.attackPower, "공격력");
        SetStatText(attackSpeed, before.attackSpeed, after.attackSpeed, "공격 속도");
        SetStatText(attackRange, before.attackRange, after.attackRange, "공격 사거리");
        SetStatText(criticalChance, before.criticalChance, after.criticalChance, "치명타 확률");
        SetStatText(criticalDamage, before.criticalDamage, after.criticalDamage, "치명타 피해");
        SetStatText(penetration, before.penetration, after.penetration, "관통력");
        SetStatText(moveSpeed, before.moveSpeed, after.moveSpeed, "이동속도");

        gold.text = rule.requiredGold.ToString();
        stone.text = rule.requiredUpgradeStones.ToString();
    }

    private float StatComparison(float current, float upgrade)
    {
        return Mathf.Approximately(upgrade, current) ? 0 : upgrade - current;
    }

    private void SetStatText(TextMeshProUGUI text, float before, float after, string label = "")
    {
        float diff = StatComparison(before, after);
        if (diff == 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
            text.text = $"{label} : {before} → {after}  (<color=green>+{diff}</color>)";
        }
    }

    private void OnUpgradeClick()
    {
        var upgradeSystem = MainSceneManager.Instance.upgrade;

        if (upgradeSystem.TryUpgrade(currectData, out var result))
        {
            Debug.Log("Upgrade 성공");
            Init(result);
        }
        else
        {
            Debug.Log("Upgrade 실패");
            Init(result);
        }
    }
}
