using UnityEngine;

public class HomeManager : Singleton<HomeManager>
{
    public GameObject canvas;
    public Inventory inventory;
    public Equipment equipment;

    public Slot selectedSlot;

    private void Awake()
    {
        canvas = Utils.InstantiatePrefabFromResource("UI/Canvas", this.transform);

        inventory = Utils.InstantiateComponentFromResource<Inventory>("UI/Inventory", canvas.transform);

        equipment ??= new();
        equipment.Init();
    }

    public void SetSelectedItem(Slot slot)
    {
        selectedSlot = slot;
    }
}
