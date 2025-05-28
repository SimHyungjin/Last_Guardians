public class SilenceDebuff : StatusEffect
{
    public SilenceDebuff(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.DeBuff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        target.isSilence = true;
        RefreshStatus(target);
    }

    public override void RemoveEffect(BaseMonster target)
    {
        target.isSilence = false;
        RefreshStatus(target);
    }
}
