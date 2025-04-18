using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerEntryUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI attackRangeText;
    public TextMeshProUGUI towerTypeText;
    public TextMeshProUGUI effectTargetText;
    public Button entryButton;

    private TowerData myData;

    public void SetData(TowerData data)
    {
        myData = data;

        towerNameText.text = data.TowerName;
        attackPowerText.text = $"공격력: {data.AttackPower}";
        attackSpeedText.text = $"공속: {data.AttackSpeed}";
        attackRangeText.text = $"사거리: {data.AttackRange}";
        towerTypeText.text = $"타입: {data.TowerType}";
        effectTargetText.text = $"타겟: {data.EffectTarget}";

        int spriteIndex = GetSpriteIndex(data.TowerIndex);
        icon.sprite = data.atlas?.GetSprite($"Tower_{spriteIndex}");

        entryButton.onClick.RemoveAllListeners();
        entryButton.onClick.AddListener(() =>
        {
            if (TowerCombinationUI.Instance.HasCombinationFor(data))
            {
                TowerCombinationUI.Instance.ShowCombinationFor(data);
                TowerCombinationUI.Instance.gameObject.SetActive(true);
                TowerCombinationUI.Instance.codexUI.LockCodexInteraction(); 
            }
        });
    }

    public Button GetButton() => entryButton;

    private int GetSpriteIndex(int index)
    {
        if (index > 49 && index < 99) return index - 49;
        if (index > 98 && index < 109) return index - 98;
        if (index > 108) return index - 59;
        return index;
    }
}
