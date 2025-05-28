using UnityEngine;

public abstract class MonsterSkillBase : MonoBehaviour
{
    public MonsterSkillData skillData;

    public abstract void UseSkill(BaseMonster caster);

}
