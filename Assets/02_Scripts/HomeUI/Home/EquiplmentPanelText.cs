using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquiplmentPanelText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attackTypeText;
    [SerializeField] private TextMeshProUGUI attackPowerText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI attackRangeText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private TextMeshProUGUI criticalChanceText;
    [SerializeField] private TextMeshProUGUI criticalDamageText;
    [SerializeField] private TextMeshProUGUI penetrationText;

    private Equipment equipment;

    private void Awake()
    {
        equipment = MainSceneManager.Instance.equipment;
        equipment.OnEquip += UpdateText;
        equipment.OnUnequip += UpdateText;

        UpdateText(equipment.GetEquipped()[EquipType.Weapon]);
    }

    private void UpdateText(ItemInstance instance)
    {
        string attackType;
        switch(equipment.changeAttackType)
        {
            case AttackType.Melee:
                attackType = "근접";
                break;
            case AttackType.Ranged:
                attackType = "화살";
                break;
            default:
                attackType = "마법";
                break;
        }
        attackTypeText.text = "공격 타입 = " + attackType;
        attackPowerText.text = "공격력 + " + equipment.totalAttack.ToString();
        attackSpeedText.text = "공격 속도 = " + equipment.totalAttackSpeed.ToString();
        attackRangeText.text = "공격 사거리 + " + equipment.totalAttackRange.ToString();
        moveSpeedText.text = "이동 속도 + " + equipment.totalMoveSpeed.ToString();
        criticalChanceText.text = "치명타 확률 + " + equipment.totalCriticalChance.ToString() + "%";
        criticalDamageText.text = "치명타 피해 + " + equipment.totalCriticalDamage.ToString() + "%";
        penetrationText.text = "관통력 + " + equipment.totalPenetration.ToString();
    }

    //private void UpdateText(ItemInstance instance)
    //{
    //    if (instance?.AsEquipData == null) return;
    //    var data = instance.AsEquipData;
    //    attackTypeText.text = data.attackType.ToString();
    //    attackPowerText.text = data.attackPower.ToString();
    //    attackSpeedText.text = data.attackSpeed.ToString();
    //    attackRangeText.text = data.attackRange.ToString();
    //    moveSpeedText.text = data.moveSpeed.ToString();
    //    criticalChanceText.text = data.criticalChance.ToString();
    //    criticalDamageText.text = data.criticalDamage.ToString();
    //    penetrationText.text = data.penetration.ToString();
    //}
}
