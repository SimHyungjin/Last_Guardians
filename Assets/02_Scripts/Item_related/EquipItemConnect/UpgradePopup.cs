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
        upgradeSlot.gameObject.SetActive(true);
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
            upgradeSlot.gameObject.SetActive(false);

            SetStatText(attackPower, 0, 0, "공격력");
            SetStatText(attackSpeed, 0, 0, "공격 속도");
            SetStatText(attackRange, 0, 0, "공격 사거리");
            SetStatText(criticalChance, 0, 0, "치명타 확률");
            SetStatText(criticalDamage, 0, 0, "치명타 피해");
            SetStatText(penetration, 0, 0, "관통력");
            SetStatText(moveSpeed, 0, 0, "이동속도");

            gold.text = "";
            stone.text = "";
            return;
        }

        EquipData before = currectData.AsEquipData;
        EquipData after = upgradeData.AsEquipData;

        SetStatText(attackPower, before.attackPower, after.attackPower, "공격력");
        SetStatText(attackSpeed, before.attackSpeed, after.attackSpeed, "공격 속도");
        SetStatText(attackRange, before.attackRange, after.attackRange, "공격 사거리");
        SetStatText(criticalChance, before.criticalChance, after.criticalChance, "치명타 확률");
        SetStatText(criticalDamage, before.criticalDamage, after.criticalDamage, "치명타 피해");
        SetStatText(penetration, before.penetration, after.penetration, "관통력");
        SetStatText(moveSpeed, before.moveSpeed, after.moveSpeed, "이동속도");

        upgradeResultText.text = $"{currectData.Data.itemGrade} → {upgradeData.Data.itemGrade}\n성공확률 : {rule.successRate}%";
        gold.text = rule.requiredGold.ToString();
        stone.text = rule.requiredUpgradeStones.ToString();
    }

    private void SetStatText(TextMeshProUGUI text, float before, float after, string label = "")
    {
        float rawDiff = after - before;
        float roundedDiff = Mathf.Round(rawDiff * 10f) / 10f;

        if (Mathf.Abs(roundedDiff) < 0.05f)
        {
            text.text = "";
            text.gameObject.SetActive(false);
            return;
        }

        string sign = roundedDiff > 0 ? "+" : "";
        text.text = $"{label} : {before:F1} → {after:F1}  (<color=green>{sign}{roundedDiff:F1}</color>)";
        text.gameObject.SetActive(true);
    }

    private void OnUpgradeClick()
    {
        var mainSceneManager = MainSceneManager.Instance;

        mainSceneManager.upgrade.TryUpgrade(currectData, out var result);

        SaveSystem.RemoveEquip(currectData.UniqueID);
        mainSceneManager.inventory.RemoveItem(currectData);

        SaveSystem.SaveEquipReward(result);
        mainSceneManager.inventory.AddItem(result);

        if (mainSceneManager.equipment.IsEquipped(currectData))
            mainSceneManager.equipment.Equip(result);

        mainSceneManager.inventoryGroup.itemPopupController.SetData(result);
        mainSceneManager.inventoryGroup.itemPopupController.UpdatePopupUI();
        mainSceneManager.inventoryGroup.selectionController.RefreshSlot(result);
        mainSceneManager.inventoryGroup.inventorySlotContainer.Display(mainSceneManager.inventory.GetFilteredView());
        Init(result);
    }
}
