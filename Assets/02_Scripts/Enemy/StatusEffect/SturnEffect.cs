using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SturnEffect : StatusEffect
{
    public SturnEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.DeBuff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        base.ApplyEffect(target);
        target.isSturn = true;
        target.SetDestination(target.transform);
    }

    public override void RemoveEffect(BaseMonster target)
    {
        base.RemoveEffect(target);
        target.isSturn = false;
    }
}
