using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        int spriteIndex = data.TowerIndex;
        if (spriteIndex > 49 && spriteIndex < 99) spriteIndex -= 49;
        else if (spriteIndex > 98 && spriteIndex < 109) spriteIndex -= 98;
        else if (spriteIndex > 108) spriteIndex -= 59;

        icon.sprite = data.atlas.GetSprite($"Tower_{spriteIndex}");
        nameText.text = data.TowerName;
    }
}
