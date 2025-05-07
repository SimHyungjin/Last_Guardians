using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObstacleDebuff : MonoBehaviour
{
    private int zoneCount = 0;
    private bool isInZone = false;
    private readonly Dictionary<Type, IPlayerBuff<PlayerData>> activeBuffs = new();

    private PlayerHandler playerHandler;
    private PlayerBuffHandler playerBuffHandler;

    public void Init()
    {
        playerHandler = GetComponent<PlayerHandler>();
        playerBuffHandler = playerHandler.playerBuffHandler;
    }

    public void EnterZone(IPlayerBuff<PlayerData> effect)
    {
        zoneCount++;

        activeBuffs[effect.GetType()] = effect;

        if (!isInZone)
        {
            isInZone = true;
            ApplyEffect(effect);
        }
    }

    public void ExitZone(IPlayerBuff<PlayerData> effect)
    {
        zoneCount = Mathf.Max(0, zoneCount - 1);
        activeBuffs.Remove(effect.GetType());

        if (zoneCount == 0 && isInZone)
        {
            isInZone = false;
            RemoveEffect(effect);
        }
    }

    private void ApplyEffect(IPlayerBuff<PlayerData> effect)
    {
        if (effect is PlayerBuffMoveSpeed)
        {
            playerHandler.playerView.InWater();
        }

        playerBuffHandler.ApplyBuff(effect);
    }

    private void RemoveEffect(IPlayerBuff<PlayerData> effect)
    {
        if (effect is PlayerBuffMoveSpeed)
        {
            playerHandler.playerView.OutWater();
        }

        playerBuffHandler.RemoveBuff(effect);
    }
}

