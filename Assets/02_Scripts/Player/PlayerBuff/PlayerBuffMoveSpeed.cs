using UnityEngine;

public class PlayerBuffMoveSpeed : IPlayerBuff<PlayerStatus>
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

    public void Apply(PlayerStatus playerData)
    {
        playerData.moveSpeed *= Value.Value;
        GameManager.Instance.PlayerManager.playerHandler.moveController.Init();
    }

    public void Remove(PlayerStatus playerData)
    {
        if (playerData == null) { Debug.LogError("playerData null"); return; }
        playerData.moveSpeed /= Value.Value;
        var moveController = GameManager.Instance.PlayerManager.playerHandler.moveController;
        if (moveController == null) { Debug.LogError("moveController null"); return; }
        moveController.Init();
    }

    public bool IsStrongerThan(IPlayerBuff<PlayerStatus> other)
    {
        if (other is not PlayerBuffMoveSpeed o) return true;

        if (IsDebuff) return this.Value < o.Value || this.Duration > o.Duration;
        else return this.Value > o.Value || this.Duration > o.Duration;
    }
}
