using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerEntryUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button entryButton;

    private TowerData myData;

    public void SetData(TowerData data)
    {
        myData = data;
        if (towerNameText != null)
            towerNameText.text = data.TowerName;
        if (descriptionText != null)
            descriptionText.text = data.TowerDescription;
        if (icon != null && TowerIconContainer.Instance != null)
            icon.sprite = TowerIconContainer.Instance.GetSprite(data.TowerIndex);
        if (entryButton != null)
        {
            entryButton.onClick.RemoveAllListeners();
            entryButton.onClick.AddListener(() =>
            {
                if (TowerCombinationUI.Instance != null &&
                    TowerCombinationUI.Instance.HasCombinationFor(myData))
                {
                    TowerCombinationUI.Instance.ShowCombinationFor(myData);
                }
            });
        }
    }
}
