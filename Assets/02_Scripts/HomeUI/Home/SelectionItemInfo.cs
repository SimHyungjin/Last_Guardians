using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectionItemInfo : MonoBehaviour
{
    [SerializeField] ItemPopupController itemPopupController;

    [SerializeField] private TextMeshProUGUI attackTypeText;
    [SerializeField] private TextMeshProUGUI attackPowerText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI attackRangeText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private TextMeshProUGUI criticalChanceText;
    [SerializeField] private TextMeshProUGUI criticalDamageText;
    [SerializeField] private TextMeshProUGUI penetrationText;

    private void OnEnable()
    {
        itemPopupController.OnItemSelected += UpdateText;
    }

    private void Start()
    {
        UpdateText(itemPopupController.currentData);
    }

    private void UpdateText(ItemInstance instance)
    {
        var data = itemPopupController.currentData?.AsEquipData;

        if (data == null)
        {
            attackTypeText.gameObject.SetActive(false);
            attackPowerText.gameObject.SetActive(false);
            attackSpeedText.gameObject.SetActive(false);
            attackRangeText.gameObject.SetActive(false);
            moveSpeedText.gameObject.SetActive(false);
            criticalChanceText.gameObject.SetActive(false);
            criticalDamageText.gameObject.SetActive(false);
            penetrationText.gameObject.SetActive(false);
            return;
        }

        if (data.equipType == EquipType.Weapon)
        {
            attackTypeText.text = $"공격 타입 : {data.attackType}";
            attackTypeText.gameObject.SetActive(true);
        }
        else
        {
            attackTypeText.gameObject.SetActive(false);
        }

        if (data.attackPower != 0)
        {
            attackPowerText.text = $"공격력 : {data.attackPower}";
            attackPowerText.gameObject.SetActive(true);
        }
        else attackPowerText.gameObject.SetActive(false);

        if (data.attackSpeed != 0)
        {
            attackSpeedText.text = $"공격 속도 : {data.attackSpeed}";
            attackSpeedText.gameObject.SetActive(true);
        }
        else attackSpeedText.gameObject.SetActive(false);

        if (data.attackRange != 0)
        {
            attackRangeText.text = $"공격 범위 : {data.attackRange}";
            attackRangeText.gameObject.SetActive(true);
        }
        else attackRangeText.gameObject.SetActive(false);

        if (data.moveSpeed != 0)
        {
            moveSpeedText.text = $"이동 속도 : {data.moveSpeed}";
            moveSpeedText.gameObject.SetActive(true);
        }
        else moveSpeedText.gameObject.SetActive(false);

        if (data.criticalChance != 0)
        {
            criticalChanceText.text = $"치명타 확률 : {data.criticalChance}%";
            criticalChanceText.gameObject.SetActive(true);
        }
        else criticalChanceText.gameObject.SetActive(false);

        if (data.criticalDamage != 0)
        {
            criticalDamageText.text = $"치명타 데미지 : {data.criticalDamage}배";
            criticalDamageText.gameObject.SetActive(true);
        }
        else criticalDamageText.gameObject.SetActive(false);

        if (data.penetration != 0)
        {
            penetrationText.text = $"관통력 : {data.penetration}";
            penetrationText.gameObject.SetActive(true);
        }
        else penetrationText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        itemPopupController.OnItemSelected -= UpdateText;
    }

    private void OnDisable()
    {
        itemPopupController.OnItemSelected -= UpdateText;
    }   

}
