using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 인벤토리 아이템 팝업 UI에서 선택된 아이템의 정보를 표시하는 클래스입니다.
/// </summary>
public class ItemPopupUIText : PopupBase
{
    [SerializeField] private TextMeshProUGUI attackTypeText;
    [SerializeField] private TextMeshProUGUI attackPowerText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI attackRangeText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private TextMeshProUGUI criticalChanceText;
    [SerializeField] private TextMeshProUGUI criticalDamageText;
    [SerializeField] private TextMeshProUGUI penetrationText;

    private ItemSelectionController selectionController;

    public override void Init()
    {
        base.Init();
        selectionController = MainSceneManager.Instance.inventoryManager.inventorySelectionController;
        selectionController.selectSlot += UpdateText;
        UpdateText(selectionController.selectedData);
    }
    /// <summary>
    /// 아이템 팝업에서 아이템을 선택했을 때 호출됩니다.
    /// 값이 있을 경우 텍스트를 업데이트합니다.
    /// 없다면 모든 텍스트를 비활성화합니다.
    /// </summary>
    /// <param name="instance"></param>
    private void UpdateText(ItemInstance instance)
    {
        var data = selectionController.selectedData?.AsEquipData;

        if (data == null)
        {
            SetStatVisibility(false);
            return;
        }

        SetStatText(attackTypeText, data.equipType == EquipType.Weapon ? $"공격 타입 : {data.attackType}" : null);
        SetStatText(attackPowerText, data.attackPower != 0 ? $"공격력 : {data.attackPower}" : null);
        SetStatText(attackSpeedText, data.attackSpeed != 0 ? $"공격 속도 : {-data.attackSpeed * 100} %" : null);
        SetStatText(attackRangeText, data.attackRange != 0 ? $"공격 범위 : {data.attackRange}" : null);
        SetStatText(moveSpeedText, data.moveSpeed != 0 ? $"이동 속도 : {data.moveSpeed}" : null);
        SetStatText(criticalChanceText, data.criticalChance != 0 ? $"치명타 확률 : {data.criticalChance}%" : null);
        SetStatText(criticalDamageText, data.criticalDamage != 0 ? $"치명타 데미지 : {data.criticalDamage}배" : null);
        SetStatText(penetrationText, data.penetration != 0 ? $"관통력 : {data.penetration}" : null);
    }

    private void SetStatText(TextMeshProUGUI text, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.text = value;
            text.gameObject.SetActive(true);
        }
    }

    private void SetStatVisibility(bool visible)
    {
        attackTypeText.gameObject.SetActive(visible);
        attackPowerText.gameObject.SetActive(visible);
        attackSpeedText.gameObject.SetActive(visible);
        attackRangeText.gameObject.SetActive(visible);
        moveSpeedText.gameObject.SetActive(visible);
        criticalChanceText.gameObject.SetActive(visible);
        criticalDamageText.gameObject.SetActive(visible);
        penetrationText.gameObject.SetActive(visible);
    }

    private void OnDestroy()
    {
        if (selectionController != null) selectionController.selectSlot -= UpdateText;
    }
}
