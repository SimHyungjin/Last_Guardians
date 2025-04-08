using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data",menuName = "Data/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("몬스터ID")]
    public int monsterID;

    [Header("스탯")]
    public float maxHP;
    public float speed;
    public float def;
    public int exp;
    public int damage;

    [Header("몬스터 설정")]
    public string monsterName;
    public string monsterDescription;


}
