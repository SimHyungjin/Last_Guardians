public class PlayerBuffMoveSpeed : IPlayerBuff<PlayerData>
{
    public float? Value { get; private set; }
    public float Duration { get; private set; }
    public bool IsDebuff { get; private set; }


    public PlayerBuffMoveSpeed(float value, float duration, bool isDebuff)
    {
        Value = value;
        Duration = duration;
        IsDebuff = isDebuff;
        if (IsDebuff) Value = 1 - value;
        else Value = 1 + value;
    }

    public void Apply(PlayerData playerData)
    {
        playerData.moveSpeed *= Value.Value;
        InGameManager.Instance.playerManager.playerController.moveController.Init();
    }

    public void Remove(PlayerData playerData)
    {
        playerData.moveSpeed /= Value.Value;
        InGameManager.Instance.playerManager.playerController.moveController.Init();
    }

    public bool IsStrongerThan(IPlayerBuff<PlayerData> other)
    {
        if (other is not PlayerBuffMoveSpeed o) return true;

        if (IsDebuff) return this.Value < o.Value || this.Duration > o.Duration;
        else return this.Value > o.Value || this.Duration > o.Duration;
    }
}
