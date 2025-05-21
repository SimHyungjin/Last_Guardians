public class DefDownEffect : StatusEffect
{
    public DefDownEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.DeBuff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        target.DeBuffDefModifier = Amount;
        RefreshStatus(target);
    }

    public override void RemoveEffect(BaseMonster target)
    {
        target.DeBuffDefModifier = 0f;
        RefreshStatus(target);
    }
}
