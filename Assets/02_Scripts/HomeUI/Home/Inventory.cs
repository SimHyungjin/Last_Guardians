using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 재고 화면과 관계된 데이터 전략 관리처.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlotContainer slotContainer;
    [SerializeField] private List<EquipmentData> inventory;

    [SerializeField] private EquipType curInventory = EquipType.Count;

    private void Awake()
    {
        inventory ??= new();
    }

    private void Start()
    {
        slotContainer.Init();
        UpdateCurInventory();
    }

    public void AddItem(EquipmentData itemdata)
    {
        inventory.Add(itemdata);
        UpdateCurInventory();
    }

    public void RemoveItem(EquipmentData itemdata)
    {
        inventory.Remove(itemdata);
        UpdateCurInventory();
    }

    public void SetInventoryType(EquipType type)
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

    public List<EquipmentData> GetFilteredInventory(EquipType type)
    {
        if (type < EquipType.Count)
        {
            List<EquipmentData> result = inventory.FindAll(item => item.equipType == type);
            result.Sort((a, b) => b.itemGrade.CompareTo(a.itemGrade));
            return result;
        }
        return new List<EquipmentData>(inventory);
    }

    public List<EquipmentData> GetInventory() => inventory;
    public InventorySlotContainer GetSlotContainer() => slotContainer;
}
