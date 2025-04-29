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
        icon.sprite = TowerManager.Instance.GetSprite(towerData.TowerIndex);
        //data.atlas.GetSprite($"Tower_{Utils.GetSpriteIndex(towerData.TowerIndex)}"); // 아이콘이 있으면
    }
}

