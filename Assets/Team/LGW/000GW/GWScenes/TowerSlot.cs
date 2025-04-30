using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;
    [SerializeField] private int slotIndex;

    public TowerData Data { get; private set; }

    private void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                TowerCombinationUI.Instance.OnClickSlot(slotIndex);
            });
        }
    }

    public void SetData(TowerData data)
    {
        Data = data;

        if (data == null)
        {
            icon.sprite = null;
            nameText.text = "???";
            return;
        }

        icon.sprite = TowerManager.Instance.GetSprite(data.TowerIndex);
        nameText.text = data.TowerName;
    }
}
