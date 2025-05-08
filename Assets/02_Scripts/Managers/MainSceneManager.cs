using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MainSceneManager는 메인 씬의 UI를 관리하는 싱글톤 클래스입니다.
/// </summary>
public class MainSceneManager : Singleton<MainSceneManager>
{
    private Dictionary<string, GameObject> panelMap = new();
    public GameObject canvas { get; private set; }
    public GameObject interactionBlocker { get; private set; }

    public Inventory inventory;
    public Equipment equipment;
    public Upgrade upgrade;
    public InventoryGroup inventoryGroup;

    private void Awake()
    {
        inventory ??= new();
        equipment ??= new();
        upgrade ??= new();

        SaveSystem.LoadGame();

        canvas = Utils.InstantiatePrefabFromResource("UI/MainScene/Canvas", this.transform);
        interactionBlocker = Utils.InstantiatePrefabFromResource("UI/MainScene/InteractionBlocker", canvas.transform);
        canvas.GetComponentInChildren<MainSceneButtonView>().Init(this);
    }


    /// <summary>
    /// 패널을 보여줍니다. 패널이 없을 경우 프리팹을 로드하여 생성합니다.
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="obj"></param>
    /// <param name="useBlocker"></param>
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

    /// <summary>
    /// 패널을 숨깁니다. 패널이 없을 경우 아무것도 하지 않습니다.
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelMap.TryGetValue(panelName, out var panel))
        {
            panel.SetActive(false);
            ShowInteractionBlocker(panel, false);
        }
    }
    /// <summary>
    /// 모든 패널을 엽니다. 패널이 없을 경우 아무것도 하지 않습니다.
    /// </summary>
    public void ShowAllPanels()
    {
        foreach (var panel in panelMap.Values)
        {
            panel.SetActive(true);
        }
    }
    /// <summary>
    /// 모든 패널을 숨깁니다. 패널이 없을 경우 아무것도 하지 않습니다.
    /// </summary>
    public void HideAllPanels()
    {
        foreach (var panel in panelMap.Values)
        {
            panel.SetActive(false);
        }
    }
    /// <summary>
    /// 상호작용 차단기를 보여줍니다.
    /// 패널이 활성화되면 패널을 가장 아래로 이동시키고, 차단기를 그 위에 배치합니다.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="active"></param>
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

    /// <summary>
    /// 인벤토리 UI를 로드합니다. 인벤토리 UI가 이미 로드된 경우 아무것도 하지 않습니다.
    /// </summary>
    /// <param name="obj"></param>
    public void LoadInventory(GameObject obj)
    {
        if (panelMap.ContainsKey("InventoryGroup")) return;

        var groupObj = Utils.InstantiatePrefabFromResource("UI/MainScene/InventoryGroup", obj.transform);
        inventoryGroup = groupObj.GetComponent<InventoryGroup>();

        upgrade.Init();
        inventoryGroup.Init();
        panelMap["InventoryGroup"] = groupObj;
    }
}