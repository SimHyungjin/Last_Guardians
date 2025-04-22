using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    private Dictionary<string, GameObject> panelMap = new();
    private GameObject canvas;

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

        SaveSystem.LoadGame();

        canvas = Utils.InstantiatePrefabFromResource("UI/MainScene/Canvas", this.transform);
        canvas.GetComponentInChildren<MainSceneButtonView>().Init(this);
    }


    public void ShowPanel(string panelName, GameObject obj = null)
    {
        if (!panelMap.TryGetValue(panelName, out var panel))
        {
            if(obj != null) panel = obj;
            else panel = Utils.InstantiatePrefabFromResource($"UI/MainScene/{panelName}", canvas.transform);
            panelMap[panelName] = panel;
        }
        panel.SetActive(true);
    }

    public void HidePanel(string panelName)
    {
        if (panelMap.TryGetValue(panelName, out var panel))
            panel.SetActive(false);
    }

    public void LoadInventory(GameObject obj)
    {
        if (panelMap.ContainsKey("InventoryGroup")) return;

        var groupObj = Utils.InstantiatePrefabFromResource("UI/MainScene/InventoryGroup", obj.transform);
        var inventoryGroup = groupObj.GetComponent<InventoryGroup>();

        inventorySlotContainer = inventoryGroup.inventorySlotContainer;
        equipmentSlotContainer = inventoryGroup.equipmentSlotContainer;
        selectionController = inventoryGroup.selectionController;
        itemPopupController = inventoryGroup.itemPopupController;

        StartCoroutine(DisplayItem());

        upgrade.Init();
        panelMap["InventoryGroup"] = groupObj;
    }

    private IEnumerator DisplayItem()
    {
        yield return new WaitForEndOfFrame();
        inventorySlotContainer.Display(inventory.GetFilteredView());
        equipmentSlotContainer.BindAll();
        equipmentSlotContainer.Refresh();
    }
}