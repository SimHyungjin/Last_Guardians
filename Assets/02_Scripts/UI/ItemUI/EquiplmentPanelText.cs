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

    private void Start()
    {
        equipment = MainSceneManager.Instance.equipment;
        equipment.OnEquip += UpdateText;
        equipment.OnUnequip += UpdateText;

        UpdateText(null);
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
        attackPowerText.text = $"공격력  {FormatStat(info.attack)}";
        attackSpeedText.text = $"공격 속도  {FormatStat(info.attackSpeed)}";
        attackRangeText.text = $"공격 사거리  {FormatStat(info.attackRange)}";
        moveSpeedText.text = $"이동 속도  {FormatStat(info.moveSpeed)}";
        criticalChanceText.text = $"치명타 확률  {FormatStat(info.criticalChance)}%";
        criticalDamageText.text = $"치명타 피해  {FormatStat(info.criticalDamage)}%";
        penetrationText.text = $"관통력  {FormatStat(info.penetration)}";
    }

    string FormatStat(float value, int decimalPlaces = 2)
    {
        string sign = value > 0 ? "+" : value < 0 ? "-" : " ";

        float absValue = Mathf.Abs(value);
        string number = absValue.ToString($"F{decimalPlaces}").TrimEnd('0').TrimEnd('.');

        return $"{sign} {number}";
    }
}
