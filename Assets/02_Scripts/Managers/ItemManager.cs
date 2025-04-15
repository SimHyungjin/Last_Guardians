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

    public ItemData GetItemData(int index)
    {
        if (!itemDatas.TryGetValue(index, out ItemData original)) return null;

        ItemData instance = Object.Instantiate(original);
        instance.GenerateUniqueID();
        return instance;
    }
}
