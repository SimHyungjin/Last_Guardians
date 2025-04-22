using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGroup : MonoBehaviour
{
    public InventorySlotContainer inventorySlotContainer;
    public EquipmentSlotContainer equipmentSlotContainer;
    public SelectionController selectionController;
    public ItemPopupController itemPopupController;

    private void OnEnable()
    {
        StartCoroutine(DelayedClose());
    }

    private IEnumerator DelayedClose()
    {
        yield return null;
        itemPopupController.Close();
    }
}
