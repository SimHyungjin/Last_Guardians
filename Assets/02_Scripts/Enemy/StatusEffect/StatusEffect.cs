using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuffDeBuff
{
    Buff,
    DeBuff
}
public abstract class StatusEffect
{
    public float Duration { get; protected set; }
    public float Amount { get; protected set; }
    public bool IsOver => Duration <= 0f;
    public BuffDeBuff BuffDeBuff { get; protected set; } = BuffDeBuff.Buff;
    protected StatusEffect(float amount, float duration)
    {
        Amount = amount;
        Duration = duration;
    }

    public virtual void UpdateEffect(BaseMonster target, float time)
    {
        Duration -= time;
        if(Duration <= 0)
        {
            RemoveEffect(target);
        }
    }

    public virtual void ApplyEffect(BaseMonster target)
    {

    }
    public virtual void RemoveEffect(BaseMonster target)
    {

    }

}
