using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSummon : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        MonsterData data = MonsterManager.Instance.MonsterDatas.Find(a => a.MonsterIndex == skillData.MonsterID);
        for (int i = 0; i < skillData.MonsterNum; i++)
        {
            Vector2 randomPos = (Vector2)caster.transform.position + Random.insideUnitCircle * skillData.SkillRange;
            NormalEnemy spwanMonster = PoolManager.Instance.Spawn(MonsterManager.Instance.NormalPrefab, caster.transform);
            spwanMonster.transform.position = randomPos;
            spwanMonster.Setup(data);
        }
    }
}
