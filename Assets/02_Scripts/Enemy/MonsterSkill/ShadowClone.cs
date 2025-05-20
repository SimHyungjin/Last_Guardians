using UnityEngine;
using UnityEngine.AI;

public class ShadowClone : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        int i = 0;
        MonsterData data = MonsterManager.Instance.MonsterDatas.Find(a => a.MonsterIndex == skillData.MonsterID);
        while (i < skillData.MonsterNum)
        {
            Vector2 randomCircle = Random.insideUnitCircle * (skillData.SkillRange / 2f);
            Vector3 randomPos = caster.transform.position + new Vector3(randomCircle.x, randomCircle.y, 0f);

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                bool isPath = NavMesh.CalculatePath(hit.position, caster.Target.transform.position, NavMesh.AllAreas, path);

                if (isPath && path.status == NavMeshPathStatus.PathComplete)
                {
                    BaseMonster baseMonster = MonsterManager.Instance.SummonMonster(skillData.MonsterID, hit.position);
                    baseMonster.Target = caster.Target;
                    baseMonster.agent.Warp(hit.position);
                    i++;
                }
            }
        }
    }


}
