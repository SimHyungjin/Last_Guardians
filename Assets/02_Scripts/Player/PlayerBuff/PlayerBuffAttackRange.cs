public class PlayerBuffAttackRange : IPlayerBuff<PlayerData>
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

    public void Apply(PlayerData playerData)
    {
        playerData.attackRange *= multiplier;
    }

    public void Remove(PlayerData playerData)
    {
        playerData.attackRange /= multiplier;
    }

    public bool IsStrongerThan(IPlayerBuff<PlayerData> other)
    {
        if (other is not PlayerBuffAttackRange o) return true;
        return this.Value > o.Value || this.Duration > o.Duration;
    }
}
