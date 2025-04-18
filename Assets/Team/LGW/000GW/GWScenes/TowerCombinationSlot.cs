using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerCombinationSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TowerData towerData;

    private void OnValidate()
    {
        if (towerData != null)
        {
            SetSlot(towerData);
        }
    }

    public void SetSlot(TowerData data)
    {
        towerData = data;
        towerNameText.text = data.TowerName;
        int spriteIndex = data.TowerIndex;
        if (towerData.TowerIndex > 49 && towerData.TowerIndex < 99)
        {
            spriteIndex = towerData.TowerIndex - 49;
        }
        else if (towerData.TowerIndex > 98 && towerData.TowerIndex < 109)
        {
            spriteIndex = towerData.TowerIndex - 98;
        }
        else if (towerData.TowerIndex > 108)
        {
            spriteIndex = towerData.TowerIndex - 59;
        }
        else
        {
            spriteIndex = towerData.TowerIndex;
        }
        icon.sprite = data.atlas.GetSprite($"Tower_{spriteIndex}"); // 아이콘이 있으면
    }
}

