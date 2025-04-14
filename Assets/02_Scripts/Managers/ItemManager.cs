using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    [SerializeField] private Dictionary<int, ItemData> itemDatas = new();

    public void LoadAllItemes()
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("SO/ItemData");
        foreach (ItemData item in allItems)
        {
            itemDatas[item.ItemIndex] = item;
        }
    }

    public ItemData GetItemData(int index)
    {
        return itemDatas.TryGetValue(index, out ItemData itemData) ? itemData : null;
    }
}
