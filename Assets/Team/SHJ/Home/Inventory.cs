using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<EquipmentData> inventory;

    public void AddItem(EquipmentData equipmentData)
    {
        inventory.Add(equipmentData);
    }

    public void EuqipmentTypeSort()
    {
        inventory = inventory.OrderBy(x => x.equipmentType).ToList();
    }

}
