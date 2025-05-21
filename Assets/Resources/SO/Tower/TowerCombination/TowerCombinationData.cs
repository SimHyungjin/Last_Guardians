using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TowerCombinationRule
{
    public int result;
    public int ingredient1;
    public int ingredient2;

    public TowerCombinationRule(int result, int ingredient1, int ingredient2)
    {
        this.result = result;
        this.ingredient1 = ingredient1;
        this.ingredient2 = ingredient2;
    }
    // 두 재료가 순서에 상관없이 일치하는지 확인하는 메서드
    public bool Matches(int ing1, int ing2)
    {
        return (ingredient1 == ing1 && ingredient2 == ing2) ||
               (ingredient1 == ing2 && ingredient2 == ing1);
    }
}

[CreateAssetMenu(fileName = "TowerCombinationData", menuName = "Data/TowerCombinationData")]
public class TowerCombinationData : ScriptableObject
{
    public List<TowerCombinationRule> combinationRules;
    public void SetData(int result, int ingredient1, int ingredient2)
    {
        TowerCombinationRule towerCombinationRule = new TowerCombinationRule(result, ingredient1, ingredient2);
        combinationRules.Add(towerCombinationRule);
    }
    // <summary>
    // 두 재료를 조합하여 결과 타워 인덱스를 반환
    // 합성 가능한 규칙이 없으면 -1을 반환
    // </summary>
    public int TryCombine(int ing1, int ing2)
    {
        foreach (var rule in combinationRules)
        {
            if (rule.Matches(ing1, ing2))
            {
                return rule.result > 0 ? rule.result : -1;
            }
        }
        return -1;
    }

}