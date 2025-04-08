using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterSkillData : ScriptableObject
{
    [Header("스킬설정")]
    public int skillID;
    public string skillName;
    public string skillDescription;

    [Header("스킬 스탯")]
    public float skillRange;
    public float skillCoolTime;

    public abstract void UseSkill();
}
