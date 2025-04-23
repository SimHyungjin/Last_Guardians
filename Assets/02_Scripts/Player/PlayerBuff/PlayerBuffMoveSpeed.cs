using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void Apply(PlayerData playerData)
    {
        playerData.moveSpeed *= Value.Value;
    }

    public void Remove(PlayerData playerData)
    {
        playerData.moveSpeed /= Value.Value;
    }

    public bool IsStrongerThan(IPlayerBuff<PlayerData> other)
    {
        if (other is not PlayerBuffMoveSpeed o) return true;

        if (IsDebuff) return this.Value < o.Value || this.Duration > o.Duration;
        else return this.Value > o.Value || this.Duration > o.Duration;
    }
}
