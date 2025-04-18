using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectSlowEffect : MonoBehaviour,ITrapEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    { Debug.Log("슬로우중"); }
}

