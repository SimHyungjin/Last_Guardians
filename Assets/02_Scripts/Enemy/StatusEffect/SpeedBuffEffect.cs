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
        base.ApplyEffect(target);
        target.BuffSpeedModifier = Amount;
        Debug.Log($"속도 {target.BuffSpeedModifier} 상승");
    }

    public override void RemoveEffect(BaseMonster target)
    {
        base.RemoveEffect(target);
        target.BuffSpeedModifier = 1f;
    }
}
