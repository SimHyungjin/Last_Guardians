using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public Inventory inventory;
    public Equipment equipment;
    public Upgrade upgrade;

    public InventorySlotContainer inventorySlotContainer;
    public EquipmentSlotContainer equipmentSlotContainer;

    public SelectionController selectionController;
    public ItemPopupController itemPopupController;

    private void Awake()
    {
        inventory ??= new();
        equipment ??= new();
        upgrade ??= new();

        var canvas = Utils.InstantiatePrefabFromResource("UI/Canvas", this.transform);
        var inventoryObj = Utils.InstantiatePrefabFromResource("UI/Inventory", canvas.transform);
        var equipmentObj = Utils.InstantiatePrefabFromResource("UI/Equipment", canvas.transform);
        var itemConnecterObj = Utils.InstantiatePrefabFromResource("UI/ItemConnecter", canvas.transform);

        inventorySlotContainer = inventoryObj.GetComponentInChildren<InventorySlotContainer>();
        equipmentSlotContainer = equipmentObj.GetComponentInChildren<EquipmentSlotContainer>();
        selectionController = itemConnecterObj.GetComponentInChildren<SelectionController>();
        itemPopupController = itemConnecterObj.GetComponentInChildren<ItemPopupController>();

        upgrade.Init();

        inventorySlotContainer.Display(inventory.GetAll());
    }
}