using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonType
{
    Standard,
    Boss,
}

public enum MonAttackPattern
{
    Melee,
    Ranged,
    Support
}

[CreateAssetMenu(fileName = "New Monster Data",menuName = "Data/Monster Data")]
public class MonsterData : ScriptableObject
{
    [SerializeField] private int monsterIndex;
    [SerializeField] private string monsterName;
    [SerializeField] private float monsterHP;
    [SerializeField] private float monsterSpeed;
    [SerializeField] private int monsterDamage;
    [SerializeField] private float monsterDef;
    [SerializeField] private int exp;
    [SerializeField] private string monsterDescription;
    [SerializeField] private MonType monsterType;
    [SerializeField] private bool hasSkill;
    [SerializeField] private int monsterSkillID;
    [SerializeField] private MonAttackPattern monsterAttackPattern;

    public int MonsterIndex => monsterIndex;
    public string MonsterName => monsterName;
    public float MonsterHP => monsterHP;
    public float MonsterSpeed => monsterSpeed;
    public int MonsterDamage => monsterDamage;
    public float MonsterDef => monsterDef;
    public int Exp => exp;
    public string MonsterDescription => monsterDescription;
    public MonType MonsterType => monsterType;
    public bool HasSkill => hasSkill;
    public int MonsterSkillID => monsterSkillID;
    public MonAttackPattern MonsterAttackPattern => monsterAttackPattern;

    public void SetData(int monsterIndex, string monsterName, float monsterHP,float monsterSpeed, int monsterDamage, float monsterDef, int exp, string monsterDescription, MonType monsterType, bool hasSkill, int monsterSkillID, MonAttackPattern attackPattern)
    {
        this.monsterIndex = monsterIndex;
        this.monsterName = monsterName;
        this.monsterHP = monsterHP;
        this.monsterSpeed = monsterSpeed;
        this.monsterDamage = monsterDamage;
        this.monsterDef = monsterDef;
        this.exp = exp;
        this.monsterDescription = monsterDescription;
        this.monsterType = monsterType;
        this.hasSkill = hasSkill;
        this.monsterSkillID = monsterSkillID;
        this.monsterAttackPattern = attackPattern;
    }
}
