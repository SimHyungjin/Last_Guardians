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
        attackPowerText.text = $"공격력: {data.AttackPower}";
        attackSpeedText.text = $"공속: {data.AttackSpeed}";
        attackRangeText.text = $"사거리: {data.AttackRange}";
        towerTypeText.text = $"타입: {data.TowerType}";
        effectTargetText.text = $"타겟: {data.EffectTarget}";

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



