using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefBuffEffect : StatusEffect
{
    public DefBuffEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.Buff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        base.ApplyEffect(target);
        target.BuffDefModifier = Amount;
    }

    public override void RemoveEffect(BaseMonster target)
    {
        base.RemoveEffect(target);
        target.BuffDefModifier = 1f;
    }
}
