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
    /// 타워 데이터를 받아서 텍스트와 아이콘을 세팅합니다.
    /// </summary>
    public void SetData(TowerData data)
    {
        myData = data;
        towerNameText.text = data.TowerName;
        descriptionText.text = data.TowerDescription;

        // ← 여기만 변경
        icon.sprite = TowerIconContainer.Instance.GetSprite(data.TowerIndex);

        entryButton.onClick.RemoveAllListeners();
        entryButton.onClick.AddListener(() =>
        {
            if (TowerCombinationUI.Instance.HasCombinationFor(myData))
                TowerCombinationUI.Instance.ShowCombinationFor(myData);
        });
    }
}
