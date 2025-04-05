public class Player
{
    public float attackPower;
    public float moveSpeed;
    public float attackSpeed;
    public float criticalChance;
    public float criticalDamage;
    public float attackRange;
    //public int[] multiTarget;
    public float penetration;
    public float elementalDamage;
    public float cooldownReduction;
    public float towerSynergy;
    public float exp;

    public Player(float attackPower, float moveSpeed, float attackSpeed, float criticalChance, float criticalDamage, float attackRange, float penetration, float elementalDamage, float cooldownReduction, float towerSynergy, float exp)
    {
        this.attackPower = attackPower;
        this.moveSpeed = moveSpeed;
        this.attackSpeed = attackSpeed;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
        this.attackRange = attackRange;
        this.penetration = penetration;
        this.elementalDamage = elementalDamage;
        this.cooldownReduction = cooldownReduction;
        this.towerSynergy = towerSynergy;
        this.exp = exp;
    }
}
