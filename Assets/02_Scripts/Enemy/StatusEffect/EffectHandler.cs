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

    public void AddEffect(StatusEffect effect)
    {
        //중복처리
        var existEffect = effects.Find(a => a.GetType() == effect.GetType());

        if(existEffect != null)
        {
            existEffect.RemoveEffect(baseMonster);
            effects.Remove(existEffect);
            effects.Add(effect);
            effect.ApplyEffect(baseMonster);
        }
        else
        {
            effects.Add(effect);
            effect.ApplyEffect(baseMonster);
        }
    }

    public void RemoveEffect(StatusEffect effect)
    {
        if (effects.Contains(effect))
        {
            effect.RemoveEffect(baseMonster);
            effects.Remove(effect);
        }
    }

    public void ClearAllEffect()
    {
        foreach(var effect in effects)
        {
            effect.RemoveEffect(baseMonster);
        }
        effects.Clear();
    }
}
