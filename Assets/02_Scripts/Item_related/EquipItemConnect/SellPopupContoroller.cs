using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellPopupContoroller : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform slotContainerView;
    [SerializeField] private List<Slot> slots = new();
    [SerializeField] private Button sellButton;
    [SerializeField] private Button apartButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI stoneText;

    private SelectionController selectionController;
    private Equipment equipment;
    private Inventory inventory;

    public Action OnSellAction;

    private void Awake()
    {
        for (int i = 0; i < 20; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/MainScene/Slot", slotContainerView);
            slots.Add(slot);
        }

        sellButton.onClick.AddListener(OnSellButtonClicked);
        //apartButton.onClick.AddListener(OnApartButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }
    private void OnEnable()
    {
        if (selectionController == null) return;

        if (selectionController.selectionMode == SelectionMode.Multi)
            SetData(null);
    }

    private void Start()
    {
        var main = MainSceneManager.Instance;
        inventory = main.inventory;
        equipment = main.equipment;
        selectionController = main.inventoryGroup.itemConnecter.selectionController;

        selectionController.SelectSlotListAction += SetData;

        if (selectionController.selectionMode == SelectionMode.Multi && selectionController.selectedDataList.Count > 0)
        {
            SetData(null);
        }
    }
    private void OnDisable()
    {
        if (selectionController == null) return;
        selectionController.ToggleMode(SelectionMode.Single);
    }


    private void OnDestroy()
    {
        if (selectionController != null) selectionController.SelectSlotListAction -= SetData;
    }

    private void SetData(Slot _)
    {
        var selectedItems = selectionController.selectedDataList;

        int totalGold = 0;
        int totalStones = 0;

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < selectedItems.Count)
            {
                var item = selectedItems[i];
                slots[i].SetData(item);
                totalGold += item.Data.ItemSellPrice;
            }
            else
            {
                slots[i].Clear();
            }
        }

        sellButton.interactable = selectedItems.Count > 0;
        apartButton.interactable = selectedItems.Count > 0;

        moneyText.text = totalGold.ToString();
        stoneText.text = totalStones.ToString();

        MainSceneManager.Instance.inventoryGroup.inventorySlotContainer.Display(MainSceneManager.Instance.inventory.GetFilteredView());
    }

    private void OnSellButtonClicked()
    {
        var selectedItems = selectionController.selectedDataList;
        int goldToAdd = 0;

        foreach (var item in selectedItems)
        {
            if (equipment.IsEquipped(item))
                equipment.UnEquip(item);

            inventory.RemoveItem(item);
            goldToAdd += item.Data.ItemSellPrice;
        }

        GameManager.Instance.gold += goldToAdd;
        SaveSystem.SaveGame();

        selectedItems.Clear();
        MainSceneManager.Instance.inventoryGroup.inventorySlotContainer.Refresh();

        OnSellAction?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnApartButtonClicked()
    {

    }
    private void OnCancelButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
