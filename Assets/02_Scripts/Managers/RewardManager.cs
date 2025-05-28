using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
    public int Gold { get; private set; }
    public int Stone { get; private set; }
    /// 이번 웨이브에서 드랍된 장비들의 ItemIndex를 모두 담음
    public List<int> EquipIndices { get; private set; } = new List<int>();

    /// 웨이브 클리어 시마다 호출
    public void GiveRewardForWave(int wave)
    {
        // 1웨이브는 튜토리얼 구간으로 보상 없음
        if (wave <= 1)
        {
            Gold = 0;
            Stone = 0;
            EquipIndices.Clear();
            return;
        }

        Gold = CalculateGold(wave);
        Stone = CalculateStone(wave);
        EquipIndices = CalculateEquips(wave);
    }

    #region 골드 보상 (50*wave ~ 100*wave)
    private int CalculateGold(int wave)
    {
        int minGold = 50 * wave;
        int maxGold = 100 * wave;
        int result = Random.Range(minGold, maxGold + 1);

        SaveSystem.SaveGetGold(result);
        return result;
    }
    #endregion

    #region 강화석 보상
    // • maxStone = 3 + ⌊wave/3⌋
    // • minStone = ⌊maxStone/2⌋
    // • dropChance = 30% + (wave/2)% → roll ≤ dropChance 이면 maxStone, 아니면 minStone
    private int CalculateStone(int wave)
    {
        int maxStone = 3 + Mathf.FloorToInt(wave / 3f);
        int minStone = Mathf.FloorToInt(maxStone / 2f);
        float dropChance = 30f + wave / 2f;
        float roll = Random.Range(0f, 100f);

        int stoneCount = (roll <= dropChance) ? maxStone : minStone;
        SaveSystem.SaveGetUpgradeStone(stoneCount);

        return stoneCount;
    }
    #endregion

    #region 장비 보상
    // • 최소 보장 개수: clamp(0,3, floor((wave–10)/15) + 1)
    // • 등급 분포: 구간별 표에 따라 랜덤
    private List<int> CalculateEquips(int wave)
    {
        var result = new List<int>();

        int minDrop = Mathf.Clamp(Mathf.FloorToInt((wave - 10) / 15f) + 1, 0, 3);
        for (int i = 0; i < minDrop; i++)
        {
            ItemGrade grade = PickGradeByWave(wave);

            // 해당 등급 아이템 후보군
            var candidates = GameManager.Instance
                .ItemManager
                .ItemDatas()
                .Values
                .Where(item => item.ItemGrade == grade)
                .ToList();

            if (candidates.Count == 0)
                continue;

            var picked = candidates[Random.Range(0, candidates.Count)];
            result.Add(picked.ItemIndex);
            SaveSystem.SaveGetItem(GameManager.Instance.ItemManager.GetItemInstanceByIndex(picked.ItemIndex));
        }

        return result;
    }

    private ItemGrade PickGradeByWave(int wave)
    {
        float r = Random.value; // 0~1

        if (wave <= 14)
        {
            // 1~14 : 일반 100%
            return ItemGrade.Normal;
        }
        else if (wave <= 30)
        {
            // 15~30 : 일반70, 희귀25, 유니크5
            if (r < 0.70f) return ItemGrade.Normal;
            if (r < 0.95f) return ItemGrade.Rare;
            return ItemGrade.Unique;
        }
        else if (wave <= 60)
        {
            // 31~60 : 일반40, 희귀35, 유니크20, 영웅5
            if (r < 0.40f) return ItemGrade.Normal;
            if (r < 0.75f) return ItemGrade.Rare;
            if (r < 0.95f) return ItemGrade.Unique;
            return ItemGrade.Hero;
        }
        else if (wave <= 90)
        {
            // 61~90 : 일반20, 희귀30, 유니크30, 영웅15, 전설5
            if (r < 0.20f) return ItemGrade.Normal;
            if (r < 0.50f) return ItemGrade.Rare;
            if (r < 0.80f) return ItemGrade.Unique;
            if (r < 0.95f) return ItemGrade.Hero;
            return ItemGrade.Legend;
        }
        else
        {
            // 91~ : 일반10, 희귀25, 유니크35, 영웅20, 전설10
            if (r < 0.10f) return ItemGrade.Normal;
            if (r < 0.35f) return ItemGrade.Rare;
            if (r < 0.70f) return ItemGrade.Unique;
            if (r < 0.90f) return ItemGrade.Hero;
            return ItemGrade.Legend;
        }
    }
    #endregion
}
