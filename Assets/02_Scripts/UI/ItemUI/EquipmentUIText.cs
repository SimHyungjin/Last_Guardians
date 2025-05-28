using TMPro;
using UnityEngine;

/// <summary>
/// 장비 패널의 텍스트를 업데이트하는 클래스입니다.
/// </summary>
public class EquipmentUIText : MonoBehaviour
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
    private PlayerBaseStatSO playerStatus;

    private void Start()
    {
        equipment = MainSceneManager.Instance.equipment;
        equipment.OnEquip += UpdateText;
        equipment.OnUnequip += UpdateText;

        playerStatus = GameManager.Instance.PlayerManager.playerStatus.playerBaseStatSO;

        UpdateText(null);
    }

    public void UpdateText(ItemInstance instance = null)
    {
        var info = equipment.InfoToPlayer();
        string attackType = info.attackType switch
        {
            AttackType.Melee => "근접",
            AttackType.Ranged => "화살",
            _ => "마법"
        };

        attackTypeText.text = $"공격 타입 = {attackType}";
        attackPowerText.text = $"공격력 = {FormatStat(playerStatus.AttackPower + info.attack)}";
        attackSpeedText.text = $"공격 속도 = {FormatStat((playerStatus.AttackSpeed - info.attackSpeed) / playerStatus.AttackSpeed * 100)}%";
        attackRangeText.text = $"공격 사거리 = {FormatStat(playerStatus.AttackRange + info.attackRange)}";
        moveSpeedText.text = $"이동 속도 = {FormatStat(playerStatus.MoveSpeed + info.moveSpeed)}";
        criticalChanceText.text = $"치명타 확률 = {FormatStat(playerStatus.CriticalChance + info.criticalChance)}%";
        criticalDamageText.text = $"치명타 피해 = {FormatStat(playerStatus.CriticalDamage + info.criticalDamage)}%";
        penetrationText.text = $"관통력 = {FormatStat(playerStatus.Penetration + info.penetration)}";
    }

    string FormatStat(float value, int decimalPlaces = 2)
    {
        float absValue = Mathf.Abs(value);
        string number = absValue.ToString($"F{decimalPlaces}").TrimEnd('0').TrimEnd('.');

        return $"{number}";
    }
}
