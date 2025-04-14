using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    private List<UpgradeRuleData> upgradeRules;

    public void Init()
    {
        upgradeRules = new List<UpgradeRuleData>(Resources.LoadAll<UpgradeRuleData>("SO/UpgradeRules"));
    }

    public bool TryUpgrade(ItemData data, out ItemData upgradedData)
    {
        upgradedData = data;

        if (!CanUpgrade(data, out var rule)) return false;

        ConsumeResources(rule);

        float roll = Random.Range(0f, 100f);
        if (roll < rule.successRate)
        {
            upgradedData = GetSuccessItem(data);
            Debug.Log($"Upgrade success: {data.itemName}");
            return true;
        }
        else
        {
            upgradedData = GetFailureItem(data, rule);
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

    private ItemData GetSuccessItem(ItemData data)
    {
        return GameManager.Instance.ItemManager.GetItemData(data.ItemIndex + 100);
    }

    private ItemData GetFailureItem(ItemData data, UpgradeRuleData rule)
    {
        if (rule.failureEffect == UpgradeFailureEffect.Downgrade)
        {
            Debug.Log("Upgrade failed, downgraded");
            return GameManager.Instance.ItemManager.GetItemData(data.ItemIndex - 100);
        }
        else
        {
            Debug.Log("Upgrade failed");
            return data;
        }
    }
}
