public class PhantomStep : MonsterSkillBase
{
    public override void UseSkill(BaseMonster caster)
    {
        caster.ApplyEvasionBuff(skillData.Duration, caster.CurrentSkillValue);
    }
}
