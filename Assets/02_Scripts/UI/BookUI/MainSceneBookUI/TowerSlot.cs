// TowerSlot.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;
    [SerializeField] private int slotIndex;

    public TowerData Data { get; private set; }

    private void Awake()
    {
        if (button != null)
            button.onClick.AddListener(() => TowerCombinationUI.Instance.OnClickSlot(slotIndex));
    }

    /// <summary>
    /// 슬롯에 타워 데이터를 세팅합니다.
    /// </summary>
    public void SetData(TowerData data)
    {
        Data = data;

        if (data == null)
        {
            icon.sprite = null;
            nameText.text = "???";
            return;
        }

        // ← 여기만 변경
        icon.sprite = TowerIconContainer.Instance.GetSprite(data.TowerIndex);
        nameText.text = data.TowerName;
    }
}
