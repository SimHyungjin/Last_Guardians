using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuffEffect : StatusEffect
{
    public SpeedBuffEffect(float amount, float duration) : base(amount, duration)
    {
        BuffDeBuff = BuffDeBuff.Buff;
    }
    public override void ApplyEffect(BaseMonster target)
    {
        target.BuffSpeedModifier = Amount;
    }

    public override void RemoveEffect(BaseMonster target)
    {
        target.BuffSpeedModifier = 0f;
    }
}
