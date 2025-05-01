using TMPro;
using UnityEngine;

/// <summary>
/// 장비 패널의 텍스트를 업데이트하는 클래스입니다.
/// </summary>
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
        var info = equipment.InfoToPlayer();

        string attackType = info.attackType switch
        {
            AttackType.Melee => "근접",
            AttackType.Ranged => "화살",
            _ => "마법"
        };

        attackTypeText.text = $"공격 타입 = {attackType}";
        attackPowerText.text = $"공격력 + {info.attack}";
        attackSpeedText.text = $"공격 속도 = {info.attackSpeed}";
        attackRangeText.text = $"공격 사거리 + {info.attackRange}";
        moveSpeedText.text = $"이동 속도 + {info.moveSpeed}";
        criticalChanceText.text = $"치명타 확률 + {info.criticalChance}%";
        criticalDamageText.text = $"치명타 피해 + {info.criticalDamage}%";
        penetrationText.text = $"관통력 + {info.penetration}";
    }
}
