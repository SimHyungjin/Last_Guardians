using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBuff
{
    float Value { get; }
    float Duration { get; }
    void Apply(PlayerData playerData);
    void Remove(PlayerData playerData);
}

public class PlayerBuff : MonoBehaviour
{
    private Dictionary<Type, (Coroutine routine, IPlayerBuff buff)> activeBuffs = new();
    private PlayerData playerData;

    public void Init()
    {
        playerData = InGameManager.Instance.playerManager.player.playerData;
    }

    public void ApplyBuff(IPlayerBuff newBuff)
    {
        Type buffType = newBuff.GetType();

        if (activeBuffs.TryGetValue(buffType, out var existing))
        {
            bool isNewBuffStronger = newBuff.Value > existing.buff.Value || newBuff.Duration > existing.buff.Duration;

            if (!isNewBuffStronger)
                return;

            StopCoroutine(existing.routine);
            existing.buff.Remove(playerData);
        }

        Coroutine routine = StartCoroutine(BuffRoutine(newBuff));
        activeBuffs[buffType] = (routine, newBuff);
    }

    private IEnumerator BuffRoutine(IPlayerBuff buff)
    {
        buff.Apply(playerData);
        yield return new WaitForSeconds(buff.Duration);
        buff.Remove(playerData);
        activeBuffs.Remove(buff.GetType());
    }
}