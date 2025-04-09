using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotContainer : MonoBehaviour
{
    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Transform ringSlot;
    [SerializeField] private Transform necklaceSlot;
    [SerializeField] private Transform helmetSlot;
    [SerializeField] private Transform armorSlot;
    [SerializeField] private Transform shoesSlot;

    public Dictionary<ItemType, Slot> slotMap = new();

    private void Awake()
    {
        slotMap[ItemType.Weapon] = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", weaponSlot);
        slotMap[ItemType.Ring] = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", ringSlot);
        slotMap[ItemType.Necklace] = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", necklaceSlot);
        slotMap[ItemType.Helmet] = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", helmetSlot);
        slotMap[ItemType.Armor] = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", armorSlot);
        slotMap[ItemType.Shoes] = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", shoesSlot);
    }

    public void RegisterSlot(ItemType type, Slot slot)
    {
        if (!slotMap.ContainsKey(type))
        {
            slotMap[type] = slot;
        }
    }

    public void BindEquipment(ItemType type, EquipmentData data)
    {
        if (slotMap.TryGetValue(type, out var slot))
        {
            slot.SetData(data);
        }
    }

    public void ClearSlot(ItemType type)
    {
        if (slotMap.TryGetValue(type, out var slot))
        {
            slot.ClearData();
        }
    }
}
