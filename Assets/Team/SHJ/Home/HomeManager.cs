using UnityEngine;

public class HomeManager : Singleton<HomeManager>
{
    public Inventory Inventory;
    public Equipment Equipment;

    private void Awake()
    {
        GameObject canvas = Utils.InstantiatePrefabFromResource("UI/Canvas");
        Inventory = Utils.InstantiateComponentFromResource<Inventory>("UI/Inventory", canvas.transform);
        Equipment ??= new();
        Utils.InstantiatePrefabFromResource("UI/Equipment",canvas.transform);
    }
}
