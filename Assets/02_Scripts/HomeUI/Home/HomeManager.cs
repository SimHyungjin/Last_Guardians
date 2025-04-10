using UnityEngine;

public class HomeManager : Singleton<HomeManager>
{
    public Inventory inventory { get; private set; }
    public Equipment equipment { get; private set; }
    public EquipmentSlotContainer equipmentSlotContainer { get; private set; }
    public SelectionController selectionController { get; private set; }

    private void Awake()
    {
        var canvas = Utils.InstantiateComponentFromResource<Canvas>("UI/Canvas", transform);

        selectionController = Utils.InstantiateComponentFromResource<SelectionController>("UI/SelectionController", canvas.transform);
        var popup = Utils.InstantiateComponentFromResource<ItemPopupController>("UI/ItemPopup", canvas.transform);
        selectionController.GetType().GetField("itemPopupController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(selectionController, popup);

        equipment = new Equipment();
        equipmentSlotContainer = Utils.InstantiateComponentFromResource<EquipmentSlotContainer>("UI/Equipment", canvas.transform);

        inventory = Utils.InstantiateComponentFromResource<Inventory>("UI/Inventory", canvas.transform);
    }
}
