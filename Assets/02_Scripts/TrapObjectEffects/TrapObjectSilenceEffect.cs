using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapObjectSilenceEffect : ITrapEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
       target= target.GetComponent<BossMonster>();
       //target.Silcence
    }
}
