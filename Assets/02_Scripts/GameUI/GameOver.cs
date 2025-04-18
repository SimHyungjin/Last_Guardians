using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameOver
{
    public int GetEquip()
    {
        int waveLevel = MonsterManager.Instance.currentWaveIndex / 5;
        if (waveLevel < 5) return 0;

        float dropChance = 5 + (float)waveLevel / 5f;
        if (waveLevel % 5 == 0) dropChance *= 1.5f;

        if (Random.Range(0f, 100f) > dropChance) return 0;

        return TryGiveRewardFromGrades(GetAllowedGrades(waveLevel));
    }

    private List<ItemGrade> GetAllowedGrades(int waveLevel)
    {
        int maxGrade = Mathf.Min((waveLevel - 1) / 20, 4);

        List<ItemGrade> grades = new();
        for (int i = 0; i <= maxGrade; i++)
            grades.Add((ItemGrade)i);

        return grades;
    }

    private int TryGiveRewardFromGrades(List<ItemGrade> allowedGrades)
    {
        var itemManager = GameManager.Instance.ItemManager;

        var candidates = itemManager.ItemDatas()
            .Values
            .Where(item => allowedGrades.Contains(item.ItemGrade))
            .ToList();

        if (candidates.Count == 0) return 0;

        var randomItem = candidates[Random.Range(0, candidates.Count)];
        SaveSystem.SaveEquipReward(randomItem.ItemIndex);
        return randomItem.ItemIndex;
    }

    public int GetUpgradeStone()
    {
        int waveLevel = MonsterManager.Instance.currentWaveIndex / 5;
        float dropChance = 5 + (float)waveLevel / 5f;
        if (waveLevel % 5 == 0) dropChance *= 1.5f;
        if (Random.Range(0f, 100f) > dropChance) return 0;

        SaveSystem.SaveUpgradeStonedReward(3 + waveLevel / 3);

        return 3 + waveLevel / 3;
    }

    public int GetGold()
    {
        int waveLevel = MonsterManager.Instance.currentWaveIndex / 5;
        int goldNum = Random.Range(50 * waveLevel, 100 * waveLevel);
        SaveSystem.SaveUpgradeStonedReward(goldNum);

        return goldNum;
    }

}
