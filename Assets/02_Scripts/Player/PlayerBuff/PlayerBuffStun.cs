using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffStun : IPlayerBuff<PlayerData>
{
    public float Value { get; private set; }
    public float Duration { get; private set; }

    private PlayerController controller;

    public PlayerBuffStun(float value, float duration, PlayerController playerController)
    {
        Value = value;
        Duration = duration;
        controller = playerController;
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
}