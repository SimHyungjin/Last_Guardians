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
    public float Duration { get; set; }
    public float Amount { get; set; }
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

    public abstract void ApplyEffect(BaseMonster target);
    public abstract void RemoveEffect(BaseMonster target);

    public void UpdateEffectTime(float amout, float duration, BaseMonster target)
    {
        //Duration = duration;
        Amount = Mathf.Max(Amount, amout);
        Duration = Mathf.Max(Amount, amout) == Amount ? Duration : duration;
        target.ApplyStatus();
    }

    protected void RefreshStatus(BaseMonster target)
    {
        target.ApplyStatus();
    }
}
