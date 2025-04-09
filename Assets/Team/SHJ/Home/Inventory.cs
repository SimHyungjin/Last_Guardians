using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 재고 화면과 관계된 데이터 전략 관리처.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlotContainer slotContainer;
    [SerializeField] private List<EquipmentData> inventory;

    [SerializeField] private ItemType curInventory = ItemType.Count;

    private void Awake()
    {
        inventory ??= new();
    }

    private void Start()
    {
        slotContainer.InitializeSlots();
        UpdateCurInventory();
    }

    public void AddItem(EquipmentData equipmentData)
    {
        inventory.Add(equipmentData);
        UpdateCurInventory();
    }

    public void RemoveItem(EquipmentData equipmentData)
    {
        inventory.Remove(equipmentData);
        UpdateCurInventory();
    }

    public void SetInventoryType(ItemType type)
    {
        if (curInventory == type) return;
        curInventory = type;
        UpdateCurInventory();
    }

    public void UpdateCurInventory()
    {
        List<EquipmentData> curView = GetFilteredInventory(curInventory);
        slotContainer.UpdateSlots(curView);
    }

    public List<EquipmentData> GetFilteredInventory(ItemType type)
    {
        if (type < ItemType.Count)
        {
            List<EquipmentData> result = inventory.FindAll(item => item.itemType == type);
            result.Sort((a, b) => b.grade.CompareTo(a.grade));
            return result;
        }
        return new List<EquipmentData>(inventory);
    }
}
