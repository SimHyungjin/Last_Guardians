using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamageEffect : StatusEffect
{
    private float tickTime = 1f; // 데미지 들어가는 주기
    private float timer; 

    public DotDamageEffect(float duration, float damage) : base(duration, damage)
    {
        timer = tickTime;
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
                target.TakeDamage(Amount);
                Debug.Log($"도트데미지 적용 {Amount} 현재체력 : {target.CurrentHP}");
                timer = tickTime;
            }
        }
        
    }

    public override void RemoveEffect(BaseMonster target)
    {
        base.RemoveEffect(target);
        Duration = 0f;
        Amount = 0f;
    }
}
