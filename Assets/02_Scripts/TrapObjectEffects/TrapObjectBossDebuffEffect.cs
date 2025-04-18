using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectBossDebuffEffect : ITrapEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    { Debug.Log("뭔가 디버프중"); }
}
