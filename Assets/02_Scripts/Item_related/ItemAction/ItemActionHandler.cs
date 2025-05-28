using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리관련 오브젝트를 연결하는 클래스입니다.
/// </summary>
public class ItemActionHandler : MonoBehaviour
{
    public Upgrade upgrade;
    public Inventory inventory;
    public Equipment equipment;

    private void Awake()
    {
        MainSceneManager mainSceneManager = MainSceneManager.Instance;
        upgrade = mainSceneManager.upgrade;
        inventory = mainSceneManager.inventory;
        equipment = mainSceneManager.equipment;
    }

    public void Equip(ItemInstance itemInstance)
    {
        if (itemInstance?.AsEquipData == null) return;
        equipment.Equip(itemInstance);
        SaveSystem.SaveEquipData();
    }

    public void UnEquip(ItemInstance itemInstance)
    {
        if (itemInstance?.AsEquipData == null) return;
        equipment.UnEquip(itemInstance);
        SaveSystem.SaveEquipData();

    }
    public ItemInstance Upgrade(ItemInstance selectItem)
    {
        upgrade.TryUpgrade(selectItem, out var result);

        bool isEquipped = equipment.IsEquipped(selectItem);

        if (isEquipped) equipment.UnEquip(selectItem, false);

        inventory.RemoveItem(selectItem,1,false);
        inventory.AddItem(result,1, false);

        if (isEquipped) equipment.Equip(result,false);

        SaveSystem.SaveEquipData();
        SaveSystem.SaveInventoryData();
        SaveSystem.SaveInventoryGoods();
        return result;
    }

    public int SellItem(List<ItemInstance> selectItems, bool money)
    {
        if (selectItems == null) return 0;
        int goods = 0;

        foreach (ItemInstance item in selectItems)
        {
            if (equipment.IsEquipped(item)) equipment.UnEquip(item);
            inventory.RemoveItem(item);
            if (money) goods += item.Data.ItemSellPrice;
            else goods += item.Data.ItemApartPrice;
        }
        if (money) GameManager.Instance.gold += goods;
        else GameManager.Instance.upgradeStones += goods;

        SaveSystem.SaveEquipData();
        SaveSystem.SaveInventoryData();
        SaveSystem.SaveInventoryGoods();
        return goods;
    }
}