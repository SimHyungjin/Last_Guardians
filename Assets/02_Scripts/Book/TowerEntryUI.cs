// TowerEntryUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerEntryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button entryButton;

    private TowerData myData;

    /// <summary>
    /// Ÿ�� �����͸� �޾Ƽ� �ؽ�Ʈ�� �������� �����մϴ�.
    /// </summary>
    public void SetData(TowerData data)
    {
        myData = data;
        towerNameText.text = data.TowerName;
        descriptionText.text = data.TowerDescription;

        // �� ���⸸ ����
        icon.sprite = TowerIconContainer.Instance.GetSprite(data.TowerIndex);

        entryButton.onClick.RemoveAllListeners();
        entryButton.onClick.AddListener(() =>
        {
            if (TowerCombinationUI.Instance.HasCombinationFor(myData))
                TowerCombinationUI.Instance.ShowCombinationFor(myData);
        });
    }
}
