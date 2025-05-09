using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerCombinationSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TowerData towerData;

    public TowerIconContainer towerIconContainer;

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
    }
}
