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
        target.isSturn = true;
        target.SetDestination(target.transform);
        target.AnimationConnect.StartSturnAnimation();
        RefreshStatus(target);
    }

    public override void RemoveEffect(BaseMonster target)
    {
        target.isSturn = false;
        target.StopSturnAnimation();
        RefreshStatus(target);
    }
}
