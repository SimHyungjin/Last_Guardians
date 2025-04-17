using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class SaveData
{
    public List<ItemInstanceSave> inventory = new();
    public List<EquippedItemSave> equipped = new();
    public int gold;
    public int upgradeStones;
}

[System.Serializable]
public class ItemInstanceSave
{
    public int itemIndex;
    public int uniqueID;
}

[System.Serializable]
public class EquippedItemSave
{
    public EquipType equipType;
    public int uniqueID;
}

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    public static void SaveReward(int itemIndex)
    {
        if (!File.Exists(SavePath))
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(new SaveData(), true));
        }

        string json = File.ReadAllText(SavePath);
        var save = JsonUtility.FromJson<SaveData>(json);

        int generateUniqueID = System.Guid.NewGuid().GetHashCode();

        save.inventory.Add(new ItemInstanceSave{itemIndex = itemIndex,uniqueID = generateUniqueID});

        string newJson = JsonUtility.ToJson(save, true);
        File.WriteAllText(SavePath, newJson);
    }

    public static void SaveGame()
    {
        var save = new SaveData();

        var inventory = HomeManager.Instance.inventory;
        var equipment = HomeManager.Instance.equipment;

        foreach (var item in inventory.GetAll())
        {
            save.inventory.Add(new ItemInstanceSave{itemIndex = item.Data.ItemIndex,uniqueID = item.UniqueID});
        }

        foreach (var kvp in equipment.GetEquipped())
        {
            save.equipped.Add(new EquippedItemSave{ equipType = kvp.Key, uniqueID = kvp.Value.UniqueID});
        }

        save.gold = GameManager.Instance.gold;
        save.upgradeStones = GameManager.Instance.upgradeStones;

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Saved to: " + SavePath);
    }

    public static void LoadGame()
    {
        if (!File.Exists(SavePath)) return;

        string json = File.ReadAllText(SavePath);
        var save = JsonUtility.FromJson<SaveData>(json);

        var itemManager = GameManager.Instance.ItemManager;
        var inventory = HomeManager.Instance.inventory;
        var equipment = HomeManager.Instance.equipment;

        inventory.ClearAll();

        Dictionary<int, ItemInstance> loadedMap = new();

        foreach (var itemSave in save.inventory)
        {
            var instance = itemManager.GetItemInstanceByIndex(itemSave.itemIndex);
            if (instance == null) continue;

            instance.OverrideUniqueID(itemSave.uniqueID);

            inventory.AddItem(instance);
            loadedMap[instance.UniqueID] = instance;
        }

        foreach (var equipSave in save.equipped)
        {
            if (loadedMap.TryGetValue(equipSave.uniqueID, out var instance))
            {
                equipment.Equip(instance);
            }
        }

        GameManager.Instance.gold = save.gold;
        GameManager.Instance.upgradeStones = save.upgradeStones;
    }
}
