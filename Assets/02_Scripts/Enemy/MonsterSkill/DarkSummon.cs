using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DarkSummon : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        int i = 0;
        MonsterData data = MonsterManager.Instance.MonsterDatas.Find(a => a.MonsterIndex == skillData.MonsterID);
        while (i < skillData.MonsterNum)
        {
            Vector2 randomPos = (Vector2)caster.transform.position + Random.insideUnitCircle * (skillData.SkillRange / 2);
            NavMeshPath path = new NavMeshPath();
            bool isPath = NavMesh.CalculatePath(randomPos, caster.Target.transform.position, NavMesh.AllAreas, path);

            if (isPath)
            {
                MonsterManager.Instance.SummonMonster(skillData.MonsterID, randomPos);
                i++;
            }
        }
    }
}
