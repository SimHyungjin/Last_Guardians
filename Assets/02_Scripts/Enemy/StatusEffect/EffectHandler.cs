using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    private List<StatusEffect> effects = new List<StatusEffect>();
    private BaseMonster baseMonster;

    private void Awake()
    {
        baseMonster = GetComponent<BaseMonster>();
    }

    private void Update()
    {
        float time = Time.deltaTime;

        for(int i = effects.Count - 1; i >= 0; i--)
        {
            var effect = effects[i];
            effect.UpdateEffect(baseMonster, time);
            if (effect.IsOver)
            {
                effects.RemoveAt(i);
            }
        }
    }

    //상태이상 추가
    public void AddEffect(StatusEffect effect)
    {
        //중복처리
        var existEffect = effects.Find(a => a.GetType() == effect.GetType());

        if(existEffect != null)
        {
            if(existEffect.Amount != effect.Amount)
            {
                existEffect.Amount = Mathf.Max(existEffect.Amount, effect.Amount);
            }
            existEffect.Duration = effect.Duration;
        }
        else
        {
            effects.Add(effect);
            effect.ApplyEffect(baseMonster);
        }
    }
    // 상태이상 제거
    public void RemoveEffect(StatusEffect effect)
    {
        if (effects.Contains(effect))
        {
            effect.RemoveEffect(baseMonster);
            effects.Remove(effect);
        }
    }

    //리스트에 이펙트가 있는지 없는지
    public bool IsInEffect(StatusEffect effect)
    {
        if (effect == null)
            return false;

        var existEffect = effects.Find(e => e.GetType() == effect.GetType());
        return existEffect != null;
    }

    // 모든상태이상 제거
    public void ClearAllEffect()
    {
        foreach(var effect in effects)
        {
            effect.RemoveEffect(baseMonster);
        }
        effects.Clear();
    }

    // 모든 버프 제거
    public void RemoveAllBuff()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if(effects[i].BuffDeBuff == BuffDeBuff.Buff)
            {
                effects[i].RemoveEffect(baseMonster);
                effects.RemoveAt(i);
            }
        }
    }

    // 모든 디버프 제거
    public void RemoveAllDeBuff()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (effects[i].BuffDeBuff == BuffDeBuff.DeBuff)
            {
                effects[i].RemoveEffect(baseMonster);
                effects.RemoveAt(i);
            }
        }
    }

    public void AllDebuffTimerPlus(float duration)
    {
        foreach(var effect in effects)
        {
            effect.Duration = duration + effect.Duration;
            Debug.Log($"디버프 시간 늘어남 {effect}, 늘어난시간 : {duration} 최종 시간 : {effect.Duration}");
        }
    }

    public List<StatusEffect> GetStatusEffects()
    {
        return effects;
    }

    public StatusEffect GetEffect(StatusEffect effect)
    {
        if(effects.Contains(effect))
            return effects.Find(a=>a.Equals(effect));
        else return effect;
    }
}
