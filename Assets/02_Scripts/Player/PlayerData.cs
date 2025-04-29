public class PlayerData
{
    public float playerIndex = 1;
    public string playerName = "라스트 가디언";
    public string playerDescription = "기본 캐릭터";

    public float baseAttackPower = 34;
    public float attackPower;

    public float baseAttackSpeed = 0.9f;
    public float attackSpeed;

    public float baseAttackRange = 1.2f;
    public float attackRange;

    public float baseMoveSpeed = 2;
    public float moveSpeed;

    public float baseCriticalChance = 10;
    public float criticalChance;

    public float baseCriticalDamage = 1.3f;
    public float criticalDamage;

    public float basePenetration = 0;
    public float penetration;

    public float baseCooldownReduction = 0;
    public float cooldownReduction;

    public int baseAbilityID = 0;
    public int abilityID;

    public AttackType attackType = AttackType.Melee;

    public PlayerData() { }

    public PlayerData(float playerIndex, string playerName, string playerDescription,
        float baseAttackPower, float baseAttackSpeed, float baseAttackRange, float baseMoveSpeed,
        float baseCriticalChance, float baseCriticalDamage, float basePenetration, float baseCooldownReduction, string towerSynergy)
    {
        this.playerIndex = playerIndex;
        this.playerName = playerName;
        this.playerDescription = playerDescription;

        this.baseAttackPower = baseAttackPower;
        this.baseAttackSpeed = baseAttackSpeed;
        this.baseAttackRange = baseAttackRange;
        this.baseMoveSpeed = baseMoveSpeed;
        this.baseCriticalChance = baseCriticalChance;
        this.baseCriticalDamage = baseCriticalDamage;
        this.basePenetration = basePenetration;
        this.baseCooldownReduction = baseCooldownReduction;

        ResetToBaseStats();
    }

    public void ResetToBaseStats()
    {
        attackPower = baseAttackPower;
        attackSpeed = baseAttackSpeed;
        attackRange = baseAttackRange;
        moveSpeed = baseMoveSpeed;
        criticalChance = baseCriticalChance;
        criticalDamage = baseCriticalDamage;
        penetration = basePenetration;
        cooldownReduction = baseCooldownReduction;
    }
}
