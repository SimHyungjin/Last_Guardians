using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameTowerEntryUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Button entryButton;

    private TowerData myData;

    /// <summary>
    /// 도감 빌드 시 Init으로 초기화
    /// </summary>
    public void Init(TowerData data)
    {
        myData = data;
        icon.sprite = TowerIconContainer.Instance.GetSprite(data.TowerIndex);
        nameText.text = data.TowerName;
        descText.text = data.TowerDescription;

        entryButton.onClick.RemoveAllListeners();
        entryButton.onClick.AddListener(OnClickEntry);
    }


    private void OnClickEntry()
    {
        
        InfoSidePanelUI.Instance?.Hide();

        
        InGameCombinationUI.Instance.ShowFor(myData);
    }



}

