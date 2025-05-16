public class PlayerBuffAttackRange : IPlayerBuff<PlayerStatus>
{
    public float? Value { get; private set; }
    public float Duration { get; private set; }

    private float multiplier;

    public PlayerBuffAttackRange(float value, float duration)
    {
        Value = value;
        Duration = duration;
        multiplier = 1f - value;
    }

    public void Apply(PlayerStatus playerData)
    {
        playerData.attackRange *= multiplier;
    }

    public void Remove(PlayerStatus playerData)
    {
        playerData.attackRange /= multiplier;
    }

    public bool IsStrongerThan(IPlayerBuff<PlayerStatus> other)
    {
        if (other is not PlayerBuffAttackRange o) return true;
        return this.Value > o.Value || this.Duration > o.Duration;
    }
}
