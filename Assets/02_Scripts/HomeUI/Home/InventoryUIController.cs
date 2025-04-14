using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventorySlotContainer inventorySlotContainer;
    [SerializeField] private InventoryUIButton inventoryUIButton;

    private void Start()
    {
        inventoryUIButton.bindInventoryBtn.AddListener((type) =>HomeManager.Instance.inventory.SetType(type));
    }
}