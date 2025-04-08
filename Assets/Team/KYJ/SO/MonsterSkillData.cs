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

    [Header("힐 수치")]
    public float healGain;

    [Header("방어력 증가")]
    public float defGain;

    [Header("몬스터 소환")]
    public int monsterID;
    public int monsterNum;

    [Header("주변 타워 스턴")]
    public float sturnDuration;

    public abstract void UseSkill();
}
