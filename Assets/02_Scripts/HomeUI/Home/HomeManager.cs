using UnityEngine;

public class HomeManager : Singleton<HomeManager>
{
    public Inventory inventory;
    public Equipment equipment;
    public Upgrade upgrade;

    public EquipmentSlotContainer equipmentSlotContainer;
    public InventorySlotContainer inventorySlotContainer;

    public SelectionController selectionController;
    public ItemPopupController itemPopupController;

    private void Awake()
    {
        equipment ??= new();
        upgrade ??= new();

        upgrade.Init();

        var canvas = Utils.InstantiatePrefabFromResource("UI/Canvas", this.transform);
        inventory = Utils.InstantiateComponentFromResource<Inventory>("UI/Inventory", canvas.transform);
        GameObject equipmentObj = Utils.InstantiatePrefabFromResource("UI/Equipment", canvas.transform);
        GameObject itemConnecterObj = Utils.InstantiatePrefabFromResource("UI/ItemConnecter", canvas.transform);
        selectionController = itemConnecterObj.GetComponentInChildren<SelectionController>();
        itemPopupController = itemConnecterObj.GetComponentInChildren<ItemPopupController>();
        equipmentSlotContainer = equipmentObj.GetComponentInChildren<EquipmentSlotContainer>();
        inventorySlotContainer = inventory.GetComponentInChildren<InventorySlotContainer>();
    }
}