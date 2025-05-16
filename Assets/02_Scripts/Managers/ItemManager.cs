using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// 아이템을 관리하는 클래스입니다.
/// </summary>
public class ItemManager
{
    private Dictionary<int, ItemData> itemDatas = new();
    private ReadOnlyDictionary<int, ItemData> readonlyItemDatas;

    /// <summary>
    /// 아이템을 로드합니다. 아이템은 Resources/Item/Equip 폴더에 있어야 합니다.
    /// </summary>
    public void LoadAllItems()
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("SO/Equip");
        foreach (ItemData item in allItems)
        {
            itemDatas[item.ItemIndex] = item;
        }

        readonlyItemDatas = new ReadOnlyDictionary<int, ItemData>(itemDatas);
    }

    /// <summary>
    /// index에 해당하는 아이템을 가져옵니다. 아이템이 없을 경우 null을 반환합니다.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ItemInstance GetItemInstanceByIndex(int index)
    {
        return itemDatas.TryGetValue(index, out var data)? new ItemInstance(data): null;
    }

    public ReadOnlyDictionary<int, ItemData> ItemDatas() => readonlyItemDatas;
}

/// <summary>
/// ItemData를 감싸기 위한 클래스입니다.
/// uniqueID와 Count를 추가합니다.
/// </summary>
public class ItemInstance
{
    public int UniqueID { get; private set; }
    public ItemData Data { get; private set; }
    public int Count { get; private set; } = 1;

    public EquipData AsEquipData => Data as EquipData;

    public ItemInstance(ItemData data)
    {
        Data = Object.Instantiate(data);
        UniqueID = System.Guid.NewGuid().GetHashCode();
    }

    public void OverrideUniqueID(int id)
    {
        UniqueID = id;
    }

    public void AddCount(int amount) => Count += amount;
    public void SubtractCount(int amount) => Count = Mathf.Max(Count - amount, 0);
    public void SetCount(int value) => Count = value;
}