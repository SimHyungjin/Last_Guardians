using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffMoveSpeed : IPlayerBuff<PlayerData>
{
    public float Value { get; private set; }
    public float Duration { get; private set; }

    public PlayerBuffMoveSpeed(float value, float duration)
    {
        Value = value;
        Duration = duration;
    }

    public void Apply(PlayerData playerData)
    {
        playerData.moveSpeed *= Value;
    }

    public void Remove(PlayerData playerData)
    {
        playerData.moveSpeed /= Value;
    }
}
