using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileNoneEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {   // No effect applied
    }
    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
    }
}