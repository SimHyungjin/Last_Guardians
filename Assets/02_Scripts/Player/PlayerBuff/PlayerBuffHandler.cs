using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBuff<T>
{
    float? Value { get; }
    float Duration { get; }
    
    void Apply(T playerData);
    void Remove(T playerData);
    //bool IsStrongerThan(IPlayerBuff<T> other);
}

public class PlayerBuffHandler : MonoBehaviour
{
    private Dictionary<Type, (Coroutine routine, IPlayerBuff<PlayerData> buff)> activeBuffs = new();
    private PlayerData playerData;

    public void Init()
    {
        playerData = InGameManager.Instance.playerManager.player.playerData;
    }

    private void OnDestroy()
    {
        ClearAllBuffs();
    }

    /// <summary>
    /// 새로운 버프를 적용합니다. 같은 타입의 기존 버프가 있다면 제거 후 덮어씌웁니다.
    /// </summary>
    public void ApplyBuff(IPlayerBuff<PlayerData> newBuff)
    {
        Type buffType = newBuff.GetType();

        if (activeBuffs.TryGetValue(buffType, out var existing))
        {
            //bool isNewBuffStronger = newBuff.IsStrongerThan(existing.buff);
            //if (!isNewBuffStronger) return;

            StopCoroutine(existing.routine);
            existing.buff.Remove(playerData);
        }

        Coroutine routine = StartCoroutine(BuffRoutine(newBuff));
        activeBuffs[buffType] = (routine, newBuff);
    }

    /// <summary>
    /// 버프 지속시간이 끝나면 자동으로 제거됩니다.
    /// </summary>
    private IEnumerator BuffRoutine(IPlayerBuff<PlayerData> buff)
    {
        buff.Apply(playerData);
        yield return new WaitForSeconds(buff.Duration);
        buff.Remove(playerData);
        activeBuffs.Remove(buff.GetType());
    }

    /// <summary>
    /// 특정 타입의 버프가 현재 적용 중인지 확인합니다.
    /// </summary>
    public bool IsBuffActive<T>() where T : IPlayerBuff<PlayerData>
    {
        return activeBuffs.ContainsKey(typeof(T));
    }

    /// <summary>
    /// 버프 인스턴스를 직접 넘겨 해당 버프를 제거합니다.
    /// </summary>
    public void RemoveBuff(IPlayerBuff<PlayerData> buff)
    {
        Type buffType = buff.GetType();
        if (activeBuffs.TryGetValue(buffType, out var existing))
        {
            StopCoroutine(existing.routine);
            existing.buff.Remove(playerData);
            activeBuffs.Remove(buffType);
        }
    }

    /// <summary>
    /// 타입 기반으로 버프를 제거합니다.
    /// </summary>
    public void RemoveBuff<T>() where T : IPlayerBuff<PlayerData>
    {
        Type buffType = typeof(T);

        if (activeBuffs.TryGetValue(buffType, out var existing))
        {
            StopCoroutine(existing.routine);
            existing.buff.Remove(playerData);
            activeBuffs.Remove(buffType);
        }
    }

    /// <summary>
    /// 모든 버프를 제거합니다.
    /// </summary>
    public void ClearAllBuffs()
    {
        foreach (var buff in activeBuffs.Values)
        {
            StopCoroutine(buff.routine);
            buff.buff.Remove(playerData);
        }
        activeBuffs.Clear();
    }
}