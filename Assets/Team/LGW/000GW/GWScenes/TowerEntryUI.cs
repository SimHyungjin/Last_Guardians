using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerEntryUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI towerNameText;
    public TextMeshProUGUI descriptionText;
    public Button entryButton;

    private TowerData myData;

    public void SetData(TowerData data)
    {
        myData = data;

        towerNameText.text = data.TowerName;
        descriptionText.text = data.TowerDescription;

        icon.sprite = TowerManager.Instance.GetSprite(data.TowerIndex);

        entryButton.onClick.RemoveAllListeners();
        entryButton.onClick.AddListener(() =>
        {
            if (TowerCombinationUI.Instance.HasCombinationFor(data))
            {
                TowerCombinationUI.Instance.ShowCombinationFor(data);
            }
        });
    }
}
