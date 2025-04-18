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

        int spriteIndex;

        if (data.TowerIndex > 49 && data.TowerIndex < 99)
        {
            spriteIndex = data.TowerIndex - 49;
        }
        else if (data.TowerIndex > 98 && data.TowerIndex < 109)
        {
            spriteIndex = data.TowerIndex - 98;
        }
        else if (data.TowerIndex > 108)
        {
            spriteIndex = data.TowerIndex - 59;
        }
        else
        {
            spriteIndex = data.TowerIndex;
        }

        icon.sprite = data.atlas?.GetSprite($"Tower_{spriteIndex}");
    }
}



