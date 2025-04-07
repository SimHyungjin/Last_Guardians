using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "Scriptable Object/Test Data", order = int.MaxValue)]
public class DataSO : ScriptableObject
{
    [SerializeField] private int index;
    [SerializeField] private string dataname;
    [SerializeField] private int hp;
    [SerializeField] private int attackPower;
    [SerializeField] private int attackSpeed;
    [SerializeField] private string description;

    public int Index => index;
    public string DataName => dataname;
    public int HP => hp;
    public int AttackPower => attackPower;
    public int AttackSpeed => attackSpeed;
    public string Description => description;

    public void SetData(int index, string dataname, int hp, int attackPower, int attackSpeed, string description)
    {
        this.index = index;
        this.dataname = dataname;
        this.hp = hp;
        this.attackPower = attackPower;
        this.attackSpeed = attackSpeed;
        this.description = description;
    }
}
