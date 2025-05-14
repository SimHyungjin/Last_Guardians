using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamageEffect : StatusEffect
{
    private float tickTime = 0.5f; // 데미지 들어가는 주기
    private float timer; 

    public DotDamageEffect(float duration, float damage) : base(duration, damage)
    {
        timer = 0;
        BuffDeBuff = BuffDeBuff.DeBuff;
    }

    public override void UpdateEffect(BaseMonster target, float time)
    {
        base.UpdateEffect(target,time);
        if (!IsOver)
        {
            timer -= time;
            if (timer <= 0f)
            {
                target.TakeDamage(Amount, 0, true);
                timer = tickTime;
            }
        }
        
    }
    public override void ApplyEffect(BaseMonster target)
    {
        RefreshStatus(target);
    }

    public override void RemoveEffect(BaseMonster target)
    {
        Duration = 0f;
        Amount = 0f;
        RefreshStatus(target);
    }
}
