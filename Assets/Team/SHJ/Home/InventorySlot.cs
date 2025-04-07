using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private SpriteRenderer sr;
    private EquipmentData data;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetSelected()
    {
        if (data == null) return;
        EquipmentManager.Instance.curEquipeedData = data;
    }

    public void UpdateSlotUI()
    {
        if (data == null)
        {
            sr.sprite = null;
        }
        else
        {
            sr.sprite = data.icon;
        }
    }
}
