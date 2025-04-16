using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TowerEntryUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI attackRangeText;
    public TextMeshProUGUI towerTypeText;
    public TextMeshProUGUI effectTargetText;

    public void SetData(TowerData data)
    {
        towerNameText.text = data.TowerName;
        attackPowerText.text = $"���ݷ�: {data.AttackPower}";
        attackSpeedText.text = $"����: {data.AttackSpeed}";
        attackRangeText.text = $"��Ÿ�: {data.AttackRange}";
        towerTypeText.text = $"Ÿ��: {data.TowerType}";
        effectTargetText.text = $"Ÿ��: {data.EffectTarget}";
        icon.sprite = data.atlas?.GetSprite(data.TowerName); // ������ ������ ǥ��
    }
}


