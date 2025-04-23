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

    public virtual void ApplyEffect(BaseMonster target)
    {

    }
    public virtual void RemoveEffect(BaseMonster target)
    {

    }

    public void UpdateEffect(float amout, float duration)
    {
        //Duration = duration;
        Amount = Mathf.Max(Amount, amout);
        Duration = Mathf.Max(Amount, amout) == Amount ? Duration : duration;
    }
}
