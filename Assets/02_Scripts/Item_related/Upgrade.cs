using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 업그레이드 시스템을 관리하는 클래스입니다.
/// </summary>
public class Upgrade
{
    private List<UpgradeRuleData> upgradeRules;

    public void Init()
    {
        upgradeRules = new List<UpgradeRuleData>(Resources.LoadAll<UpgradeRuleData>("SO/UpgradeRules"));
    }
    /// <summary>
    /// 업그레이드 시도합니다. 업그레이드 성공 여부에 따라 업그레이드된 인스턴스를 반환합니다.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="upgradedInstance"></param>
    /// <returns></returns>
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
            Debug.Log($"Upgrade success: {instance.Data.ItemName}");
            return true;
        }
        else
        {
            var failedData = GetFailureItem(instance.Data, rule);
            upgradedInstance = failedData;
            return false;
        }
    }
    /// <summary>
    /// 업그레이드 가능한지 확인합니다. 업그레이드 규칙을 찾고, 필요한 자원이 있는지 확인합니다.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="rule"></param>
    /// <returns></returns>
    private bool CanUpgrade(ItemData data, out UpgradeRuleData rule)
    {
        rule = upgradeRules.Find(x => x.sourceGrade == data.ItemGrade);
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
    /// <summary>
    /// 업그레이드에 필요한 자원을 소비합니다. 업그레이드 규칙에 따라 자원을 차감합니다.
    /// </summary>
    /// <param name="rule"></param>
    private void ConsumeResources(UpgradeRuleData rule)
    {
        GameManager.Instance.gold -= rule.requiredGold;
        GameManager.Instance.upgradeStones -= rule.requiredUpgradeStones;
        SaveSystem.SaveGetGold(-rule.requiredGold);
        SaveSystem.SaveGetUpgradeStone(-rule.requiredUpgradeStones);
    }
    /// <summary>
    /// 업그레이드 성공 시 업그레이드된 아이템을 반환합니다. 업그레이드 규칙에 따라 업그레이드된 아이템을 찾습니다.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private ItemInstance GetSuccessItem(ItemInstance data)
    {
        int upgradeIndex = data.AsEquipData.ItemIndex + 100;
        var upgradeData = GameManager.Instance.ItemManager.GetItemInstanceByIndex(upgradeIndex);
        return upgradeData;
    }
    /// <summary>
    /// 업그레이드 실패 시 업그레이드된 아이템을 반환합니다. 업그레이드 규칙에 따라 업그레이드된 아이템을 찾습니다.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="rule"></param>
    /// <returns></returns>
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

    public IReadOnlyList<UpgradeRuleData> GetUpgradeRules() { return upgradeRules; }
}
