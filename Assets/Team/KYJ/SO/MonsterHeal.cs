using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealSkilData",menuName = "Data/HealSkilData")]
public class MonsterHeal : MonsterSkillData
{
    [Header("힐 수치")]
    public float healGain;
    public override void UseSkill()
    {
        //주변 사거리에 있는 
    }
}
