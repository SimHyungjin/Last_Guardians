using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePopupController : PopupBase
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

    public override void Init()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClick);
        cancelButton.onClick.AddListener(() => Close());
        RefreshText();
    }

    public override void Open()
    {
        base.Open();
        //SetData(currectData);
    }

    public override void Close()
    {
        base.Close();
        currectData = null;
        upgradeData = null;
    }


    public void SetData(ItemInstance instance)
    {
        NeedInit();
        currectData = instance;
        upgradeData = null;
        if (currectData != null && currectData.AsEquipData != null)
        {
            currentSlot.SetData(currectData);
            upgradeData = GameManager.Instance.ItemManager.GetItemInstanceByIndex(currectData.AsEquipData.ItemIndex + 100);
            upgradeSlot.SetData(upgradeData);
            upgradeSlot.gameObject.SetActive(true);
        }
        else
        {
            currentSlot.Clear();
            upgradeSlot.Clear();
            upgradeSlot.gameObject.SetActive(false);
        }
        RefreshText();
    }


    private void RefreshText()
    {
        if(currectData == null)
        {
            EmptyText();
            upgradeResultText.text = "선택된 장비가 없습니다";
            return;
        }
        if(upgradeData == null)
        {
            EmptyText();
            upgradeResultText.text = "최종 형태입니다.";
            return;
        }
        upgradeButton.interactable = true;
        var upgradeSystem = MainSceneManager.Instance.upgrade;
        var rule = upgradeSystem.GetUpgradeRules().FirstOrDefault(x => x.sourceGrade == currectData.Data.ItemGrade);

        if (rule == null)
        {
            EmptyText();
            upgradeResultText.text = "최종 상태";
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

        upgradeResultText.text = $"{currectData.Data.ItemGrade} → {upgradeData.Data.ItemGrade}\n성공확률 : {rule.successRate}%";
        gold.text = rule.requiredGold.ToString();
        stone.text = rule.requiredUpgradeStones.ToString();
    }

    private void EmptyText()
    {
        upgradeButton.interactable = false;
        upgradeSlot.gameObject.SetActive(false);

        upgradeResultText.text = "";
        SetStatText(attackPower, 0, 0, "공격력");
        SetStatText(attackSpeed, 0, 0, "공격 속도");
        SetStatText(attackRange, 0, 0, "공격 사거리");
        SetStatText(criticalChance, 0, 0, "치명타 확률");
        SetStatText(criticalDamage, 0, 0, "치명타 피해");
        SetStatText(penetration, 0, 0, "관통력");
        SetStatText(moveSpeed, 0, 0, "이동속도");

        gold.text = "";
        stone.text = "";
    }

    private void SetStatText(TextMeshProUGUI text, float before, float after, string label = "")
    {
        float rawDiff = after - before;
        float roundedDiff = Mathf.Round(rawDiff * 100f) / 100f;

        if (Mathf.Abs(roundedDiff) < 0.01f)
        {
            text.text = "";
            text.gameObject.SetActive(false);
            return;
        }

        bool isAttackSpeed = label.Contains("공격 속도");
        string sign = roundedDiff > 0 ? "+" : "-";

        string color = isAttackSpeed
            ? (roundedDiff > 0 ? "red" : "green")
            : (roundedDiff > 0 ? "green" : "red");

        string beforeStr = before.ToString("0.##");
        string afterStr = after.ToString("0.##");
        string diffStr = $"{sign}{Mathf.Abs(roundedDiff).ToString("0.##")}";

        text.text = $"{label} : {beforeStr} → {afterStr}  (<color={color}>{diffStr}</color>)";
        text.gameObject.SetActive(true);
    }

    private void OnUpgradeClick()
    {
        var main = MainSceneManager.Instance;

        main.upgrade.TryUpgrade(currectData, out var result);

        SaveSystem.RemoveEquip(currectData.UniqueID);
        main.inventory.RemoveItem(currectData);

        SaveSystem.SaveEquipReward(result);
        main.inventory.AddItem(result);

        if (main.equipment.IsEquipped(currectData))
            main.equipment.Equip(result);

        main.inventoryGroup.itemConnecter.itemPopupController.SetData(result);
        main.inventoryGroup.itemConnecter.itemPopupController.UpdatePopupUI();
        main.inventoryGroup.itemConnecter.selectionController.RefreshSlot(result);
        main.inventoryGroup.inventorySlotContainer.Display(main.inventory.GetFilteredView());

        SetData(result);
    }
}
