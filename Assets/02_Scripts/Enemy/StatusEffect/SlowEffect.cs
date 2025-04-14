using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : StatusEffect
{
    public SlowEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.DeBuff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        base.ApplyEffect(target);
        target.DeBuffSpeedModifier = Amount;
    }

    public override void RemoveEffect(BaseMonster target)
    {
        base.RemoveEffect(target);
        target.DeBuffSpeedModifier = 1f;
    }
}
