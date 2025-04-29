using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : Singleton<MainSceneManager>
{
    private Dictionary<string, GameObject> panelMap = new();
    public GameObject canvas { get; private set; }
    public GameObject interactionBlocker { get; private set; }

    public Inventory inventory;
    public Equipment equipment;
    public Upgrade upgrade;

    public InventorySlotContainer inventorySlotContainer;
    public EquipmentSlotContainer equipmentSlotContainer;

    public SelectionController selectionController;
    public ItemPopupController itemPopupController;

    private void Awake()
    {
        // TowerManager 자동 생성
        if (FindObjectOfType<TowerManager>() == null)
        {
            GameObject towerManagerPrefab = Resources.Load<GameObject>("Prefabs/TowerManager");
            if (towerManagerPrefab != null)
            {
                var go = Instantiate(towerManagerPrefab);
                go.name = "TowerManager"; // 이름 정리
                DontDestroyOnLoad(go);    // 씬 전환 유지
            }
            else
            {
                Debug.LogError("TowerManager 프리팹이 Resources/Prefabs 폴더에 없습니다!");
            }
        }

        inventory ??= new();
        equipment ??= new();
        upgrade ??= new();

        SaveSystem.LoadGame();

        canvas = Utils.InstantiatePrefabFromResource("UI/MainScene/Canvas", this.transform);
        interactionBlocker = Utils.InstantiatePrefabFromResource("UI/MainScene/InteractionBlocker", canvas.transform);
        canvas.GetComponentInChildren<MainSceneButtonView>().Init(this);
    }



    public void ShowPanel(string panelName, GameObject obj = null, bool useBlocker = true)
    {
        if (!panelMap.TryGetValue(panelName, out var panel))
        {
            if (obj != null) panel = obj;
            else panel = Utils.InstantiatePrefabFromResource($"UI/MainScene/{panelName}", canvas.transform);

            panelMap[panelName] = panel;
        }

        panel.SetActive(true);

        var childCanvas = panel.GetComponentInChildren<Canvas>(true);
        if (childCanvas != null) childCanvas.enabled = true;

        if (useBlocker)
            ShowInteractionBlocker(panel, true);
        else
            interactionBlocker.SetActive(false);

        panel.transform.SetSiblingIndex(panel.transform.parent.childCount - 1);
    }


    public void HidePanel(string panelName)
    {
        if (panelMap.TryGetValue(panelName, out var panel))
        {
            panel.SetActive(false);
            ShowInteractionBlocker(panel, false);
        }
    }

    public void ShowAllPanels()
    {
        foreach (var panel in panelMap.Values)
        {
            panel.SetActive(true);
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in panelMap.Values)
        {
            panel.SetActive(false);
        }
    }

    public void ShowInteractionBlocker(GameObject obj, bool active)
    {
        if (active)
        {
            var parent = obj.transform.parent;
            obj.transform.SetSiblingIndex(parent.childCount - 1);
            interactionBlocker.transform.SetParent(parent, false);
            interactionBlocker.transform.SetSiblingIndex(parent.childCount - 2);
            interactionBlocker.SetActive(true);
        }
        else interactionBlocker.SetActive(false);
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