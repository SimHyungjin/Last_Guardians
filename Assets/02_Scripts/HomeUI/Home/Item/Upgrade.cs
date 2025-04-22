using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    private List<UpgradeRuleData> upgradeRules;

    public void Init()
    {
        upgradeRules = new List<UpgradeRuleData>(Resources.LoadAll<UpgradeRuleData>("SO/UpgradeRules"));
    }

    public bool TryUpgrade(ItemInstance instance, out ItemInstance upgradedInstance)
    {
        upgradedInstance = instance;

        if (!CanUpgrade(instance.Data, out var rule)) return false;

        ConsumeResources(rule);

        float roll = Random.Range(0f, 100f);
        if (roll < rule.successRate)
        {
            var upgradedData = GetSuccessItem(instance);
            upgradedInstance = upgradedData;
            Debug.Log($"Upgrade success: {instance.Data.itemName}");
            return true;
        }
        else
        {
            var failedData = GetFailureItem(instance.Data, rule);
            upgradedInstance = failedData;
            return false;
        }
    }

    private bool CanUpgrade(ItemData data, out UpgradeRuleData rule)
    {
        rule = upgradeRules.Find(x => x.sourceGrade == data.itemGrade);
        if (rule == null)
        {
            Debug.Log("No upgrade rule found");
            return false;
        }

        if (GameManager.Instance.gold < rule.requiredGold)
        {
            Debug.Log("Not enough gold");
            return false;
        }

        if (GameManager.Instance.upgradeStones < rule.requiredUpgradeStones)
        {
            Debug.Log("Not enough upgrade stones");
            return false;
        }

        return true;
    }

    private void ConsumeResources(UpgradeRuleData rule)
    {
        GameManager.Instance.gold -= rule.requiredGold;
        GameManager.Instance.upgradeStones -= rule.requiredUpgradeStones;
    }

    private ItemInstance GetSuccessItem(ItemInstance data)
    {
        int upgradeIndex = data.AsEquipData.ItemIndex + 100;
        SaveSystem.RemoveEquip(data.UniqueID);
        var upgradeData = GameManager.Instance.ItemManager.GetItemInstanceByIndex(upgradeIndex);
        SaveSystem.SaveEquipReward(upgradeData.AsEquipData.ItemIndex);
        return upgradeData;
    }

    private ItemInstance GetFailureItem(ItemData data, UpgradeRuleData rule)
    {
        if (rule.failureEffect == UpgradeFailureEffect.Downgrade)
        {
            Debug.Log("Upgrade failed, downgraded");
            return GameManager.Instance.ItemManager.GetItemInstanceByIndex(data.ItemIndex - 100);
        }
        else
        {
            Debug.Log("Upgrade failed");
            return new ItemInstance(data);
        }
    }
}
