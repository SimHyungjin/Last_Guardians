public class SlowEffect : StatusEffect
{
    public SlowEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.DeBuff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        target.DeBuffSpeedModifier = Amount;
        RefreshStatus(target);
    }

    public override void RemoveEffect(BaseMonster target)
    {
        target.DeBuffSpeedModifier = 0f;
        RefreshStatus(target);
    }
}
