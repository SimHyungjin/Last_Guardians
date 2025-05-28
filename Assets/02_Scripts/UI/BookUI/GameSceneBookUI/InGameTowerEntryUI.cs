using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameTowerEntryUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Button entryButton;

    private TowerData myData;

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
        List<TowerData> selected = MulliganUI.Instance != null
            ? MulliganUI.Instance.GetSelectedCards()
            : null;
        InGameCombinationUI.Instance.ShowFor(myData, selected);
    }
}
