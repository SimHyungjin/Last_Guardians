using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 이 클래스는 장비 아이템을 관리하고,
/// 현재 선택된 장비 타입(ItemType)에 따라 필터링된 목록을 UI 슬롯에 표시하는 역할을 합니다.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private List<EquipmentData> inventory;
    [SerializeField] private List<InventorySlot> slots;

    [SerializeField] private ItemType curInventory;

    [SerializeField] private InventorySlot slot;
    [SerializeField] private int slotNum = 10;

    private void Awake()
    {
        inventory ??= new();
        slots ??= new();
    }

    private void Start()
    {
        for (int i = 0; i < slotNum; i++)
        {
            slots.Add(Instantiate(slot, this.transform));
        }
        curInventory = ItemType.Count;
        UpdateCurInventory();
    }

    /// <summary>
    /// 새로운 아이템을 인벤토리에 추가하고, 현재 선택된 타입이라면 UI에 반영합니다.
    /// </summary>
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

    /// <summary>
    /// 장비 타입 탭(무기, 방어구 등)을 변경합니다.
    /// </summary>
    public void SetInventoryType(ItemType type)
    {
        if (curInventory == type) return;
        curInventory = type;
        UpdateCurInventory();
    }

    /// <summary>
    /// 현재 선택된 장비 타입에 맞춰 필터링된 인벤토리를 슬롯 UI에 표시합니다.
    /// </summary>
    public void UpdateCurInventory()
    {
        List<EquipmentData> curView = GetFilteredInventory(curInventory);
        RefleshSlot(curView);
    }

    /// <summary>
    /// 필터링된 리스트를 받아 슬롯 UI를 갱신합니다.
    /// </summary>
    public void RefleshSlot(List<EquipmentData> view)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < view.Count)
                slots[i].SetData(view[i]);
            else
                slots[i].ClearData();
        }
    }

    /// <summary>
    /// 특정 장비 타입에 해당하는 아이템들만 골라 정렬된 리스트로 반환합니다.
    /// </summary>
    public List<EquipmentData> GetFilteredInventory(ItemType type)
    {
        if (type < ItemType.Count)
        {
            List<EquipmentData> result = new();
            foreach (var item in inventory)
            {
                if (item.itemType == type)
                    result.Add(item);
            }
            result.Sort((a, b) => b.grade.CompareTo(a.grade));
            return result;
        }
        return inventory;
    }
}
