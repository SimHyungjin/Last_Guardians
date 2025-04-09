using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterSkillType
{
    Buff,
    DeBuff,
    Evasion,
    Disable,
    Summon
}

[CreateAssetMenu(fileName = "New Monster Skill Data", menuName = "Data/Monster Skill Data")]
public class MonsterSkillData : ScriptableObject
{
    [SerializeField] private int skillIndex;
    [SerializeField] private string skillName;
    [SerializeField] private MonsterSkillType skillType;
    [SerializeField] private float skillRange;
    [SerializeField] private string skillDescription;
    [SerializeField] private float duration;
    [SerializeField] private float skillCoolTime;
    [SerializeField] private float monsterskillEffectValue;
    [SerializeField] private float monsterskillProbablilty;
    [SerializeField] private int monsterID;
    [SerializeField] private int monsterNum;

    public int SkillIndex => skillIndex;
    public string SkillName => skillName;
    public MonsterSkillType SkillType => skillType;
    public float SkillRange => skillRange;
    public string SkillDescription => skillDescription;
    public float Duration => duration;
    public float SkillCoolTime => skillCoolTime;
    public float MonsterskillEffectValue => monsterskillEffectValue;
    public float MonsterskillProbablilty => monsterskillProbablilty;
    public int MonsterID => monsterID;
    public int MonsterNum => monsterNum;

    public void SetData(int monsterskillIndex, string monsterskillName, MonsterSkillType monsterskillType, float monsterskillRange, int spawnMonsterID, int spawnMonsterNum, float monsterskillDuration, float monsterskillEffectValue, string monsterskillDescripttion, float monsterskillProbablilty, float monsterskillCoolTime)
    {
        this.skillIndex = monsterskillIndex;
        this.skillName = monsterskillName;
        this.skillType = monsterskillType;
        this.skillRange = monsterskillRange;
        this.monsterID = spawnMonsterID;
        this.monsterNum = spawnMonsterNum;
        this.duration = monsterskillDuration;
        this.monsterskillEffectValue = monsterskillEffectValue;
        this.skillDescription = monsterskillDescripttion;
        this.monsterskillProbablilty = monsterskillProbablilty;
        this.skillCoolTime = monsterskillCoolTime;
    }

    //public abstract void UseSkill();
}
