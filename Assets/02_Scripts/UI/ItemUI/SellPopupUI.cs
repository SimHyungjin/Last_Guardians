using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellPopupUI : PopupBase
{
    [Header("UI References")]
    [SerializeField] private Transform slotContainerView;
    [SerializeField] private List<Slot> slots = new();
    [SerializeField] private Button sellButton;
    [SerializeField] private Button apartButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI stoneText;

    private EquipmentSlotContainer equipmentSlotContainer;
    private InventorySlotContainer inventorySlotContainer;
    private ItemActionHandler itemActionHandler;
    private ItemSelectionController selectionController;

    public Action onSellAction;

    public override void Init()
    {
        base.Init();
        var main = MainSceneManager.Instance;
        equipmentSlotContainer = main.inventoryManager.equipmentSlotContainer;
        inventorySlotContainer = main.inventoryManager.inventorySlotContainer;
        itemActionHandler = main.inventoryManager.itemActionHandler;
        selectionController = main.inventoryManager.inventorySelectionController;

        for (int i = 0; i < 20; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/MainScene/Slot", slotContainerView);
            slots.Add(slot);
        }

        sellButton.onClick.AddListener(() => OnSellButtonClicked(true));
        apartButton.onClick.AddListener(() => OnSellButtonClicked(false));
        cancelButton.onClick.AddListener(Close);

        selectionController.selectSlotListAction += RefreshSellPopupUI;
        Close();
    }

    public override void Open()
    {
        if (selectionController == null) return;
        selectionController.ToggleMode(SelectionMode.Multi);
        selectionController.ClearSelect();
        selectionController.ClearSelects();
        RefreshSellPopupUI();
        base.Open();
    }

    public override void Close()
    {
        if (selectionController == null) return;
        selectionController.ToggleMode(SelectionMode.Single);
        foreach (var slot in slots)
        {
            slot.Clear();
        }
        base.Close();
    }

    private void RefreshSellPopupUI()
    {
        var selectedItems = selectionController.selectedDataList;

        int totalGold = 0;
        int totalStones = 0;

        if (selectedItems.Count == 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Clear();
            }
        }
        else
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (i < selectedItems.Count)
                {
                    var item = selectedItems[i];
                    slots[i].SetData(item);
                    totalGold += item.Data.ItemSellPrice;
                    totalStones += item.Data.ItemApartPrice;
                }
                else
                {
                    slots[i].Clear();
                }
            }
        }

        sellButton.interactable = selectedItems.Count > 0;
        apartButton.interactable = selectedItems.Count > 0;

        moneyText.text = totalGold.ToString();
        stoneText.text = totalStones.ToString();

        inventorySlotContainer.Display();
    }

    private void OnSellButtonClicked(bool money)
    {
        itemActionHandler.SellItem(selectionController.selectedDataList, money);
        selectionController.ClearSelects();

        equipmentSlotContainer.Refresh();
        inventorySlotContainer.Display();
        Close();

        onSellAction?.Invoke();
    }
}
