using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<ItemInstanceSave> inventory = new();
    public List<EquippedItemSave> equipped = new();
    public SerializableTowerUpgradeData towerUpgradedata;
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


    private static void ModifySaveData(Action<SaveData> modifier)
    {
        if (!File.Exists(SavePath))
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(new SaveData(), true));
        }

        string json = File.ReadAllText(SavePath);
        var save = JsonUtility.FromJson<SaveData>(json);

        modifier?.Invoke(save);

        File.WriteAllText(SavePath, JsonUtility.ToJson(save, true));
    }

    public static void SaveGetItem(ItemInstance instance)
    {
        ModifySaveData(save =>
        {
            save.inventory.Add(new ItemInstanceSave
            {
                itemIndex = instance.Data.ItemIndex,
                uniqueID = instance.UniqueID
            });
        });
    }

    public static void SaveGetGold(int gold)
    {
        ModifySaveData(save =>
        {
            save.gold += gold;
        });
    }

    public static void SaveGetUpgradeStone(int upgradestones)
    {
        ModifySaveData(save =>
        {
            save.upgradeStones += upgradestones;
        });
    }

    public static void SaveRemoveEquip(int uniqueID)
    {
        ModifySaveData(save =>
        {
            save.inventory.RemoveAll(item => item.uniqueID == uniqueID);
            save.equipped.RemoveAll(equip => equip.uniqueID == uniqueID);
        });
    }

    public static void SaveRemoveItem(int uniqueID)
    {
        ModifySaveData(save =>
        {
            save.inventory.RemoveAll(item => item.uniqueID == uniqueID);
        });
    }

    public static void SaveEquipData()
    {

        ModifySaveData(save =>
        {
            save.equipped.Clear();
            foreach (var item in MainSceneManager.Instance.equipment.GetEquipped())
            {
                save.equipped.Add(new EquippedItemSave
                {
                    equipType = item.Key,
                    uniqueID = item.Value.UniqueID
                });
            }
        });
    }

    public static void SaveInventoryData()
    {
        ModifySaveData(save =>
        {
            save.inventory.Clear();
            foreach (var item in MainSceneManager.Instance.inventory.GetAll())
            {
                save.inventory.Add(new ItemInstanceSave
                {
                    itemIndex = item.Data.ItemIndex,
                    uniqueID = item.UniqueID
                });
            }
        });
    }

    public static void SaveInventoryGoods()
    {
        ModifySaveData(save =>
        {
            save.gold = GameManager.Instance.gold;
            save.upgradeStones = GameManager.Instance.upgradeStones;
        });
    }

    public static void SaveTowerUpgradeData(TowerUpgrade towerUpgrade)
    {
        ModifySaveData(save =>
        {
            save.towerUpgradedata = new SerializableTowerUpgradeData(towerUpgrade.towerUpgradeData);
        });
    }

    public static void SaveGame()
    {
        SaveInventoryData();
        SaveEquipData();
        SaveInventoryGoods();
        SaveTowerUpgradeData(MainSceneManager.Instance.TowerUpgrade);
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
        var towerUpgrade = MainSceneManager.Instance.TowerUpgrade;

        inventory.ClearAll();
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

        if (save.towerUpgradedata == null)
        {
            Debug.LogWarning("[SaveSystem] 타워 업그레이드 데이터 없음, 로드 스킵");
            return;
        }
        if (towerUpgrade == null)
        {
            Debug.LogWarning("towerUpgrade없음");
        }
        if (towerUpgrade.towerUpgradeData == null)
        {
            Debug.LogWarning("towerUpgradeData없음");
        }
        else
        {
            towerUpgrade.towerUpgradeData.totalMasteryPoint = save.towerUpgradedata.totalMasteryPoint;
            towerUpgrade.towerUpgradeData.currentMasteryPoint = save.towerUpgradedata.currentMasteryPoint;
            towerUpgrade.towerUpgradeData.usedMasteryPoint = save.towerUpgradedata.usedMasteryPoint;
            towerUpgrade.towerUpgradeData.towerPoint = save.towerUpgradedata.towerPoint;
            towerUpgrade.towerUpgradeData.currentLevel = save.towerUpgradedata.currentLevel;
        }

        GameManager.Instance.gold = save.gold;
        GameManager.Instance.upgradeStones = save.upgradeStones;
        Debug.Log($"[SaveSystem] 최종 상태 적용 - 골드: {save.gold}, 강화석: {save.upgradeStones}");
    }
}
