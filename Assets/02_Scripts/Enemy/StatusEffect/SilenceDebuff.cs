using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilenceDebuff : StatusEffect
{
    public SilenceDebuff(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.DeBuff;
    }

    public override void ApplyEffect(BaseMonster target)
    {
        base.ApplyEffect(target);
        target.isSilence = true;
    }

    public override void RemoveEffect(BaseMonster target)
    {
        base.RemoveEffect(target);
        target.isSilence = false;
    }
}
