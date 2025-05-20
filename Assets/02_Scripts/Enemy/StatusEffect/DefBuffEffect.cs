public class DefBuffEffect : StatusEffect
{
    public DefBuffEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.Buff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        target.BuffDefModifier = Amount;
        RefreshStatus(target);
    }

    public override void RemoveEffect(BaseMonster target)
    {
        target.BuffDefModifier = 0f;
        RefreshStatus(target);
    }
}
