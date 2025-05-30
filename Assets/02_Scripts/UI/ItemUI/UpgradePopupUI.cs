using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

enum UpgradeResultType 
{   
    Success, 
    Same,
    Downgrade 
}

public class UpgradePopupUI : PopupBase
{
    [SerializeField] private Slot currentSlot;
    [SerializeField] private Slot upgradeSlot;

    [SerializeField] private GameObject upgradeEffectObj;
    [SerializeField] private Image upgradeEffectImage;
    [SerializeField] private Image crashEffectImage;

    [SerializeField] private TextMeshProUGUI upgradeResultText;
    [SerializeField] private TextMeshProUGUI downgradeText;
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

    [SerializeField] private EquipmentUIText equipmentUIText;
    private EquipmentSlotContainer equipmentSlotContainer;
    private InventorySlotContainer inventorySlotContainer;
    private ItemActionHandler actionHandler;
    private ItemSelectionController selectionController;
    private Upgrade upgrade;

    ItemInstance currentData;
    ItemInstance upgradeData;
    ItemInstance updateData;

    public Action onUpgradeAction;

    public override void Init()
    {
        var inventoryManager = MainSceneManager.Instance.inventoryManager;
        equipmentSlotContainer = inventoryManager.equipmentSlotContainer;
        inventorySlotContainer = inventoryManager.inventorySlotContainer;
        actionHandler = inventoryManager.itemActionHandler;
        selectionController = inventoryManager.inventorySelectionController;
        upgrade = MainSceneManager.Instance.upgrade;


        upgradeButton.onClick.RemoveListener(OnUpgradeClick);
        upgradeButton.onClick.AddListener(OnUpgradeClick);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(Close);

        RefreshText();
    }

    public override void Open()
    {
        base.Open();
        SetData(selectionController.selectedData);
    }

    public override void Close()
    {
        upgradeData = null;
        base.Close();
    }


    public void SetData(ItemInstance instance)
    {
        currentData = instance;
        upgradeData = null;
        if (currentData != null && currentData.AsEquipData != null)
        {
            currentSlot.SetData(currentData);
            upgradeData = GameManager.Instance.ItemManager.GetItemInstanceByIndex(currentData.AsEquipData.ItemIndex + 100);
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
        if (currentData == null)
        {
            EmptyText();
            upgradeResultText.text = "선택된 장비가 없습니다";
            return;
        }
        if (upgradeData == null)
        {
            EmptyText();
            upgradeResultText.text = "최종 형태입니다.";
            return;
        }
        var rule = upgrade.GetUpgradeRules().FirstOrDefault(x => x.sourceGrade == currentData.Data.ItemGrade);
        if (rule == null)
        {
            EmptyText();
            upgradeResultText.text = "최종 상태";
            return;
        }

        EquipData before = currentData.AsEquipData;
        EquipData after = upgradeData.AsEquipData;

        SetStatText(attackPower, before.attackPower, after.attackPower, "공격력");
        SetStatText(attackSpeed, before.attackSpeed, after.attackSpeed, "공격 속도");
        SetStatText(attackRange, before.attackRange, after.attackRange, "공격 사거리");
        SetStatText(criticalChance, before.criticalChance, after.criticalChance, "치명타 확률");
        SetStatText(criticalDamage, before.criticalDamage, after.criticalDamage, "치명타 피해");
        SetStatText(penetration, before.penetration, after.penetration, "관통력");
        SetStatText(moveSpeed, before.moveSpeed, after.moveSpeed, "이동속도");

        upgradeResultText.text = $"{currentData.Data.ItemGrade} → {upgradeData.Data.ItemGrade}\n성공확률 : {rule.successRate}%";

        if (upgradeData.Data.ItemGrade == ItemGrade.Legend || upgradeData.Data.ItemGrade == ItemGrade.Hero)
            downgradeText.gameObject.SetActive(true);
        else
            downgradeText.gameObject.SetActive(false);

        gold.text = rule.requiredGold.ToString();
        stone.text = rule.requiredUpgradeStones.ToString();
        if (upgrade.CanUpgrade(currentData.Data)) upgradeButton.interactable = true;
        else upgradeButton.interactable = false;
    }

    private void EmptyText()
    {
        upgradeButton.interactable = false;
        upgradeSlot.gameObject.SetActive(false);
        downgradeText.gameObject.SetActive(false);
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
        updateData = actionHandler.Upgrade(currentData);

        Color color = Color.white;
        switch (upgradeData.Data.ItemGrade)
        {
            case ItemGrade.Normal: color = upgradeSlot.normalColor; break;
            case ItemGrade.Rare: color = upgradeSlot.rareColor; break;
            case ItemGrade.Unique: color = upgradeSlot.uniqueColor; break;
            case ItemGrade.Hero: color = upgradeSlot.heroColor; break;
            case ItemGrade.Legend: color = upgradeSlot.legendColor; break;
        }

        bool isSuccess = updateData.Data.ItemGrade > currentData.Data.ItemGrade; 
        float targetFill = isSuccess ? 1f : 0.3f + UnityEngine.Random.Range(0, 5) * 0.1f;

        StartCoroutine(SetEffectCoroutine(isSuccess, targetFill, color));
    }

    IEnumerator SetEffectCoroutine(bool success, float targetFill, Color color)
    {
        upgradeEffectImage.fillAmount = 0;
        upgradeEffectImage.color = color;
        upgradeEffectObj.gameObject.SetActive(true);

        float fillDuration = targetFill;
        float pauseDuration = 1f - targetFill;

        float timer = 0f;

        while (timer < fillDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fillDuration);
            upgradeEffectImage.fillAmount = Mathf.Lerp(0f, targetFill, t);
            if(!success && timer > targetFill && !crashEffectImage.gameObject.activeSelf) crashEffectImage.gameObject.SetActive(true);
            yield return null;
        }
        if (pauseDuration > 0)
            yield return new WaitForSeconds(pauseDuration);
        yield return new WaitForSeconds(0.2f);
        crashEffectImage.gameObject.SetActive(false);
        upgradeEffectObj.gameObject.SetActive(false);

        selectionController.SetSelected(updateData);
        SetData(updateData);
        equipmentUIText.UpdateText();
        equipmentSlotContainer.BindAll();
        equipmentSlotContainer.Refresh();
        inventorySlotContainer.Display();
        RefreshText();

        onUpgradeAction?.Invoke();
    }
}
