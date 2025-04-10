using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryUIButton inventoryUIButton;

    private void Start()
    {
        inventoryUIButton.bindInventoryBtn.AddListener(inventory.SetType);
    }
}
