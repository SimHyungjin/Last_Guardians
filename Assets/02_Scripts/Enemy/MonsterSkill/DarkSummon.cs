using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSummon : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        Utils.Shuffle(MonsterManager.Instance.NavMeshModifiers);
        foreach (var modifier in MonsterManager.Instance.NavMeshModifiers)
        {
            if (Vector2.Distance(caster.transform.position, modifier.transform.position) <= skillData.SkillRange / 2)
            {
                MonsterManager.Instance.nearbyModifiers.Add(modifier);

                if (MonsterManager.Instance.nearbyModifiers.Count > skillData.MonsterNum)
                    break;
            }
        }

        MonsterData data = MonsterManager.Instance.MonsterDatas.Find(a => a.MonsterIndex == skillData.MonsterID);
        for (int i = 0; i < skillData.MonsterNum; i++)
        {
            if (i >= MonsterManager.Instance.nearbyModifiers.Count)
                MonsterManager.Instance.SummonMonster(skillData.MonsterID, MonsterManager.Instance.nearbyModifiers[i - MonsterManager.Instance.nearbyModifiers.Count].transform.position);
            else
                MonsterManager.Instance.SummonMonster(skillData.MonsterID, MonsterManager.Instance.nearbyModifiers[i].transform.position);
        }

        MonsterManager.Instance.nearbyModifiers.Clear();
    }
}
