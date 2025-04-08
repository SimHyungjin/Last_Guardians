using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    //// �� ��ᰡ ������ ������� ��ġ�ϴ��� Ȯ���ϴ� �޼���
    //public bool Matches(int ing1, int ing2)
    //{
    //    return (ingredient1 == ing1 && ingredient2 == ing2) ||
    //           (ingredient1 == ing2 && ingredient2 == ing1);
    //}
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
    /// <summary>
    /// �� ��Ḧ �����Ͽ� ��� Ÿ�� �ε����� ��ȯ
    /// �ռ� ������ ��Ģ�� ������ -1�� ��ȯ
    /// </summary>
    //    public int TryCombine(int ing1, int ing2)
    //    {
    //        foreach (var rule in combinationRules)
    //        {
    //            if (rule.Matches(ing1, ing2))
    //            {
    //                return rule.result > 0 ? rule.result : -1;
    //            }
    //        }
    //        return -1;
    //    }
}