using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    [SerializeField] private Dictionary<int, ItemData> itemDatas = new();

    public void LoadAllItems()
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("SO/Equip");
        foreach (ItemData item in allItems)
        {
            itemDatas[item.ItemIndex] = item;
        }
    }

    public ItemInstance GetItemData(ItemData data)
    {
        if (data == null) return null;

        return itemDatas.TryGetValue(data.ItemIndex, out var itemData) ? new ItemInstance(itemData): null;
    }

    public ItemInstance GetItemInstanceByIndex(int index)
    {
        return itemDatas.TryGetValue(index, out var data)? new ItemInstance(data): null;
    }
}

public class ItemInstance
{
    public int UniqueID { get; private set; }
    public ItemData Data { get; private set; }

    public EquipData asEquipData => Data as EquipData;

    public ItemInstance(ItemData data)
    {
        Data = data;
        UniqueID = System.Guid.NewGuid().GetHashCode();
    }
}
