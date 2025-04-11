using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    public List<UpgradeRuleData> upgradeRules;

    public void Init()
    {
        upgradeRules = new List<UpgradeRuleData>(Resources.LoadAll<UpgradeRuleData>("SO/UpgradeRules"));
    }

    public bool TryUpgarade(ItemData data)
    {
        var rule = upgradeRules.Find(x => x.sourceGrade == data.itemGrade);
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
        if(GameManager.Instance.upgradeStones < rule.requiredUpgradeStones)
        {
            Debug.Log("Not enough upgrade stones");
            return false;
        }

        GameManager.Instance.gold -= rule.requiredGold;
        GameManager.Instance.upgradeStones -= rule.requiredUpgradeStones;

        float random = Random.Range(0f, 100f);
        if (random < rule.successRate)
        {
            data.itemGrade = rule.targetGrade;
            Debug.Log($"Upgrade success {data.itemGrade}");
            return true;
        }
        else
        {
            if(rule.failureEffect == UpgradeFailureEffect.Downgrade)
            {
                data.itemGrade = (ItemGrade)Mathf.Max(0, (int)data.itemGrade - 1);
                Debug.Log("Upgrade failed, downgraded");
            }
            else
            {
                Debug.Log("Upgrade failed");
            }
            return false;
        }
    }
}
