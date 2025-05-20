public class SkillValueDebuffEffect : StatusEffect
{
    public SkillValueDebuffEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.DeBuff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        if (target.MonsterData.HasSkill)
        {
            target.SkillValueModifier = Amount;
            RefreshStatus(target);
        }
    }

    public override void RemoveEffect(BaseMonster target)
    {
        if (target.MonsterData.HasSkill)
        {
            target.SkillValueModifier = 1f;
            RefreshStatus(target);
        }
    }
}
