using UnityEngine.UI;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private EquipmentData data;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void SetData(EquipmentData newData)
    {
        data = newData;
        UpdateSlotUI();
    }

    public void ClearData()
    {
        data = null; 
        UpdateSlotUI();
    }

    public void SelectEquip()
    {
        if (data == null) return;
        Equipment.Instance.curEquippedData = data;
    }

    public void UpdateSlotUI()
    {
        if (data == null)
        {
            icon.sprite = null;
        }
        else
        {
            icon.sprite = data.icon;
        }
    }
}
