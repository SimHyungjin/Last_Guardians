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

    public static void SaveEquipReward(int itemIndex)
    {
        Debug.Log($"[SaveSystem] SaveEquipReward 실행 - itemIndex: {itemIndex}");

        if (!File.Exists(SavePath))
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(new SaveData(), true));
            Debug.Log("[SaveSystem] save.json이 없어서 새로 생성함.");
        }

        string json = File.ReadAllText(SavePath);
        var save = JsonUtility.FromJson<SaveData>(json);

        int generateUniqueID = System.Guid.NewGuid().GetHashCode();

        save.inventory.Add(new ItemInstanceSave{itemIndex = itemIndex,uniqueID = generateUniqueID});
        Debug.Log($"[SaveSystem] 아이템 추가 - uniqueID: {generateUniqueID}");

        string newJson = JsonUtility.ToJson(save, true);
        File.WriteAllText(SavePath, newJson);
    }
    public static void RemoveEquip(int uniqueID)
    {
        Debug.Log($"[SaveSystem] RemoveEquipReward 실행 - uniqueID: {uniqueID}");
        if (!File.Exists(SavePath))
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(new SaveData(), true));
            Debug.Log("[SaveSystem] save.json이 없어서 새로 생성함.");
        }
        string json = File.ReadAllText(SavePath);
        var save = JsonUtility.FromJson<SaveData>(json);
        save.inventory.RemoveAll(item => item.uniqueID == uniqueID);
        Debug.Log($"[SaveSystem] 아이템 제거 - uniqueID: {uniqueID}");
        string newJson = JsonUtility.ToJson(save, true);
        File.WriteAllText(SavePath, newJson);
    }

    public static void SaveGoldReward(int gold)
    {
        Debug.Log($"[SaveSystem] SaveGoldReward 실행 - 추가 골드: {gold}");

        if (!File.Exists(SavePath))
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(new SaveData(), true));
            Debug.Log("[SaveSystem] save.json이 없어서 새로 생성함.");
        }

        string json = File.ReadAllText(SavePath);
        var save = JsonUtility.FromJson<SaveData>(json);

        save.gold += gold;
        Debug.Log($"[SaveSystem] 총 골드: {save.gold}");

        string newJson = JsonUtility.ToJson(save, true);
        File.WriteAllText(SavePath, newJson);
    }

    public static void SaveUpgradeStonedReward(int upgradeStone)
    {
        Debug.Log($"[SaveSystem] SaveUpgradeStonedReward 실행 - 추가 강화석: {upgradeStone}");

        if (!File.Exists(SavePath))
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(new SaveData(), true));
            Debug.Log("[SaveSystem] save.json이 없어서 새로 생성함.");
        }

        string json = File.ReadAllText(SavePath);
        var save = JsonUtility.FromJson<SaveData>(json);

        save.upgradeStones += upgradeStone;
        Debug.Log($"[SaveSystem] 총 강화석: {save.upgradeStones}");

        string newJson = JsonUtility.ToJson(save, true);
        File.WriteAllText(SavePath, newJson);
    }

    public static void SaveGame()
    {
        Debug.Log("[SaveSystem] SaveGame 실행");

        var save = new SaveData();

        var inventory = MainSceneManager.Instance.inventory;
        var equipment = MainSceneManager.Instance.equipment;

        foreach (var item in inventory.GetAll())
        {
            save.inventory.Add(new ItemInstanceSave { itemIndex = item.Data.ItemIndex, uniqueID = item.UniqueID });
            Debug.Log($"[SaveSystem] 인벤토리 저장 - index: {item.Data.ItemIndex}, uniqueID: {item.UniqueID}");
        }

        foreach (var kvp in equipment.GetEquipped())
        {
            save.equipped.Add(new EquippedItemSave { equipType = kvp.Key, uniqueID = kvp.Value.UniqueID });
            Debug.Log($"[SaveSystem] 장비 저장 - {kvp.Key} : uniqueID: {kvp.Value.UniqueID}");
        }

        save.gold = GameManager.Instance.gold;
        save.upgradeStones = GameManager.Instance.upgradeStones;
        Debug.Log($"[SaveSystem] 골드: {save.gold}, 강화석: {save.upgradeStones}");

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("[SaveSystem] 저장 완료 → " + SavePath);
    }

    public static void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("[SaveSystem] 저장 파일 없음, 로드 스킵");
            return;
        }

        Debug.Log("[SaveSystem] LoadGame 실행");

        string json = File.ReadAllText(SavePath);
        var save = JsonUtility.FromJson<SaveData>(json);

        var itemManager = GameManager.Instance.ItemManager;
        var inventory = MainSceneManager.Instance.inventory;
        var equipment = MainSceneManager.Instance.equipment;

        inventory.ClearAll(false);
        Debug.Log("[SaveSystem] 인벤토리 초기화");

        Dictionary<int, ItemInstance> loadedMap = new();

        foreach (var itemSave in save.inventory)
        {
            var instance = itemManager.GetItemInstanceByIndex(itemSave.itemIndex);
            if (instance == null)
            {
                Debug.LogWarning($"[SaveSystem] 잘못된 아이템 인덱스: {itemSave.itemIndex}, 무시함");
                continue;
            }

            instance.OverrideUniqueID(itemSave.uniqueID);
            inventory.AddItem(instance, 1, false);
            loadedMap[instance.UniqueID] = instance;

            Debug.Log($"[SaveSystem] 인벤토리 로드 - index: {itemSave.itemIndex}, uniqueID: {itemSave.uniqueID}");
        }

        foreach (var equipSave in save.equipped)
        {
            if (loadedMap.TryGetValue(equipSave.uniqueID, out var instance))
            {
                equipment.Equip(instance, false);
                Debug.Log($"[SaveSystem] 장비 로드 - {equipSave.equipType} : uniqueID: {equipSave.uniqueID}");
            }
            else
            {
                Debug.LogWarning($"[SaveSystem] 장비 로드 실패 - uniqueID {equipSave.uniqueID} 를 인벤토리에서 찾을 수 없음");
            }
        }

        GameManager.Instance.gold = save.gold;
        GameManager.Instance.upgradeStones = save.upgradeStones;
        Debug.Log($"[SaveSystem] 최종 상태 적용 - 골드: {save.gold}, 강화석: {save.upgradeStones}");
    }
}
