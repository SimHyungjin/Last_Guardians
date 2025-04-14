using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefDownEffect : StatusEffect
{
    public DefDownEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.DeBuff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        base.ApplyEffect(target);
        target.DeBuffDefModifier = Amount;
    }

    public override void RemoveEffect(BaseMonster target)
    {
        base.RemoveEffect(target);
        target.DeBuffDefModifier = 1f;
    }
}
