using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasionBuffEffect : StatusEffect
{
    public EvasionBuffEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.Buff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        target.EvasionRate = Amount;
    }

    public override void RemoveEffect(BaseMonster target)
    {
        target.EvasionRate = -1f;
    }
}
