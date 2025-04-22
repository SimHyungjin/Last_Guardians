using System.Linq;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
    public int GetGold(int wave)
    {
        int minGold = 50 * wave;
        int maxGold = 100 * wave;
        int result = Random.Range(minGold, maxGold + 1);

        SaveSystem.SaveGoldReward(result);
        return result;
    }

    public int GetUpgradeStone(int wave)
    {
        int minStone = Mathf.Clamp(wave / 10 + 1, 1, 30); // ex) 1~30
        int maxStone = Mathf.Clamp(wave / 5 + 3, 3, 40);  // ex) 3~40
        int result = Random.Range(minStone, maxStone + 1);

        SaveSystem.SaveUpgradeStonedReward(result);
        return result;
    }

    public int GetEquip(int wave)
    {
        float dropChance = Mathf.Max(0, (wave - 4) * 0.5f); // wave 5부터 드랍 시작 (0 → 0.5 → ...)
        if (Random.Range(0f, 100f) > dropChance) return 0;

        int grade = Mathf.Clamp((wave - 1) / 20, 0, 5);
        return TryGiveEquipReward(grade);
    }

    private int TryGiveEquipReward(int grade)
    {
        var candidates = GameManager.Instance.ItemManager.ItemDatas()
            .Values
            .Where(item => (int)item.ItemGrade == grade)
            .ToList();

        if (candidates.Count == 0) return 0;

        var selected = candidates[Random.Range(0, candidates.Count)];
        SaveSystem.SaveEquipReward(selected.ItemIndex);
        return selected.ItemIndex;
    }

    public void GiveRewardForWave(int wave)
    {
        Debug.Log($"[RewardManager] 웨이브 {wave} 보상 지급 시작");

        int gold = GetGold(wave);
        int stone = GetUpgradeStone(wave);
        int equip = GetEquip(wave);

        Debug.Log($"[RewardManager] 골드: {gold}, 강화석: {stone}, 장비: {(equip != 0 ? equip.ToString() : "없음")}");
    }
}
