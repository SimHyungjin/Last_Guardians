using UnityEngine;

public class PlayerStatus
{
    public PlayerBaseStatSO playerBaseStatSO { get; private set; }

    public Sprite icon = null;
    public AttackType attackType = AttackType.Melee;
    public float attackPower = 0;
    public float attackSpeed = 0;
    public float attackRange = 0;
    public float moveSpeed = 0;
    public float criticalChance = 0;
    public float criticalDamage = 0;
    public float penetration = 0;
    public float cooldownReduction = 0;
    public int abilityID = 0;
    public int specialEffectID = 0;


    public void Init()
    {
        playerBaseStatSO = new();
    }

    public void SetStatus()
    {
        var equipment = GameManager.Instance.PlayerManager.equipmentStat;
        if (playerBaseStatSO == null || equipment == null) return;

        icon = equipment.icon;
        attackType = equipment.attackType;
        attackPower = playerBaseStatSO.AttackPower + equipment.attack;
        attackSpeed = playerBaseStatSO.AttackSpeed + equipment.attackSpeed;
        attackRange = playerBaseStatSO.AttackRange + equipment.attackRange;
        moveSpeed = playerBaseStatSO.MoveSpeed + equipment.moveSpeed;
        criticalChance = playerBaseStatSO.CriticalChance + equipment.criticalChance;
        criticalDamage = playerBaseStatSO.CriticalDamage + equipment.criticalDamage;
        penetration = playerBaseStatSO.Penetration + equipment.penetration;
    }
}
