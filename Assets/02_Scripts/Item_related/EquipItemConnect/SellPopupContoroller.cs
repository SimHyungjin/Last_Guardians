using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SellPopupContoroller : PopupBase
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

    public override void Init()
    {
        base.Init();
        var main = MainSceneManager.Instance;
        inventory = main.inventory;
        equipment = main.equipment;
        selectionController = main.inventoryGroup.itemConnecter.selectionController;

        for (int i = 0; i < 20; i++)
        {
            var slot = Utils.InstantiateComponentFromResource<Slot>("UI/MainScene/Slot", slotContainerView);
            slots.Add(slot);
        }

        sellButton.onClick.AddListener(OnSellButtonClicked);
        // apartButton.onClick.AddListener(OnApartButtonClicked);
        cancelButton.onClick.AddListener(Close);
        selectionController.SelectSlotListAction += SetData;
    }

    public override void Open()
    {
        base.Open();

        if (selectionController.selectionMode != SelectionMode.Multi)
            selectionController.ToggleMode(SelectionMode.Multi);

        SetData(null);
    }

    public override void Close()
    {
        base.Close();
        if (selectionController == null) return;
        selectionController.ToggleMode(SelectionMode.Single);
    }

    private void SetData(Slot _)
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
        Close();
    }

    private void OnApartButtonClicked()
    {

    }
}
