using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBuff<T>
{
    float Value { get; }
    float Duration { get; }
    void Apply(T playerData);
    void Remove(T playerData);
}

public class PlayerBuffHandler : MonoBehaviour
{
    private Dictionary<Type, (Coroutine routine, IPlayerBuff<PlayerData> buff)> activeBuffs = new();
    private PlayerData playerData;

    public void Init()
    {
        playerData = InGameManager.Instance.playerManager.player.playerData;
    }

    public void ApplyBuff(IPlayerBuff<PlayerData> newBuff)
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

    private IEnumerator BuffRoutine(IPlayerBuff<PlayerData> buff)
    {
        buff.Apply(playerData);
        yield return new WaitForSeconds(buff.Duration);
        buff.Remove(playerData);
        activeBuffs.Remove(buff.GetType());
    }
}