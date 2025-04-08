using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : BaseMonster
{
    protected override void Attack()
    {
        Debug.Log("노말공격");
        attackTimer = attackDelay;
    }
}
