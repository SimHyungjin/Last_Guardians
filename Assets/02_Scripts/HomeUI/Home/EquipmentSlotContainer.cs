using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EquipmentSlotContainer : MonoBehaviour
{
    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Transform ringSlot;
    [SerializeField] private Transform necklaceSlot;
    [SerializeField] private Transform helmetSlot;
    [SerializeField] private Transform armorSlot;
    [SerializeField] private Transform shoesSlot;

    public Dictionary<EquipType, Slot> slotMap = new();

    private void Awake()
    {
        slotMap[EquipType.Weapon] = CreateSlot(EquipType.Weapon, weaponSlot);
        slotMap[EquipType.Ring] = CreateSlot(EquipType.Ring, ringSlot);
        slotMap[EquipType.Necklace] = CreateSlot(EquipType.Necklace, necklaceSlot);
        slotMap[EquipType.Helmet] = CreateSlot(EquipType.Helmet, helmetSlot);
        slotMap[EquipType.Armor] = CreateSlot(EquipType.Armor, armorSlot);
        slotMap[EquipType.Shoes] = CreateSlot(EquipType.Shoes, shoesSlot);
    }

    private Slot CreateSlot(EquipType type, Transform parent)
    {
        var slot = Utils.InstantiateComponentFromResource<Slot>("UI/Slot", parent);
        return slot;
    }

    public void BindEquipment(EquipType type, EquipemntData data)
    {
        if (slotMap.TryGetValue(type, out var slot))
        {
            slot.SetData(data);
        }
    }

    public void ClearSlot(EquipType type)
    {
        if (slotMap.TryGetValue(type, out var slot))
        {
            slot.ClearData();
        }
    }
}
