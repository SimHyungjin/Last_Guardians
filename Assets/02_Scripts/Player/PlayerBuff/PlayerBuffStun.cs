using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffStun : IPlayerBuff<PlayerData>
{
    public float? Value => null;
    public float Duration { get; private set; }

    private PlayerController controller;

    public PlayerBuffStun(float duration, PlayerController controller)
    {
        Duration = duration;
        this.controller = controller;
    }

    public void Apply(PlayerData playerData)
    {
        controller.attackController.AutoAttackStop();
        controller.moveController.SetCanMove(false);
    }

    public void Remove(PlayerData playerData)
    {
        controller.attackController.AutoAttackStart();
        controller.moveController.SetCanMove(true);
    }

    public bool IsStrongerThan(IPlayerBuff<PlayerData> other)
    {
        if (other is not PlayerBuffStun o) return true;
        return this.Duration > o.Duration;
    }
}