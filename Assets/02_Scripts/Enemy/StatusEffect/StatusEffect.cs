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
    public float OriginDuration { get; set; }
    public bool IsOver => Duration <= 0f;
    public BuffDeBuff BuffDeBuff { get; protected set; } = BuffDeBuff.Buff;
    protected StatusEffect(float amount, float duration)
    {
        Amount = amount;
        Duration = duration;
        OriginDuration = duration;
    }

    public virtual void UpdateEffect(BaseMonster target, float time)
    {
        Duration -= time;
        if (Duration <= 0)
        {
            RemoveEffect(target);
        }
    }

    public abstract void ApplyEffect(BaseMonster target);
    public abstract void RemoveEffect(BaseMonster target);

    public void UpdateEffectTime(float amount, float duration, BaseMonster target)
    {
        //Duration = duration;
        Amount = Mathf.Max(Amount, amount);
        Duration = Mathf.Max(Amount, amount) == amount ? duration : Duration;
        OriginDuration = Duration;
        target.ApplyStatus();
    }

    protected void RefreshStatus(BaseMonster target)
    {
        target.ApplyStatus();
    }
}
