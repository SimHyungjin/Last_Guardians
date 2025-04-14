public class PlayerData
{
    public float playerIndex = 1;
    public string playerName = "라스트 가디언";
    public string playerDescription = "기본 캐릭터";

    public float attackPower = 20;
    public float attackSpeed = 0.1f;
    public float attackRange = 2;

    public float moveSpeed = 2;

    public float criticalChance = 10;
    public float criticalDamage = 1.3f;

    public float penetration = 5;
    public float cooldownReduction = 0;
    public string towerSynergy = string.Empty;

    public PlayerData() { }
    public PlayerData(float playerIndex, string playerName, string playerDescription, float attackPower,
        float attackSpeed, float attackRange, float moveSpeed, float criticalChance, float criticalDamage, float penetration, float cooldownReduction, string towerSynergy)
    {
        this.playerIndex = playerIndex;
        this.playerName = playerName;
        this.playerDescription = playerDescription;
        this.attackPower = attackPower;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.moveSpeed = moveSpeed;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
        this.penetration = penetration;
        this.cooldownReduction = cooldownReduction;
        this.towerSynergy = towerSynergy;
    }
}
