public class Player
{
    public PlayerData playerData {  get; private set; }

    public void Init()
    {
        if(playerData == null) playerData = new();
    }

    public void SetStatus()
    {
        var equipment = HomeManager.Instance.equipment;
        if (playerData == null || equipment == null) return;

        playerData.attackPower += equipment.totalAttack;
        playerData.attackSpeed += equipment.totalAttackSpeed;
        playerData.attackRange += equipment.totalAttackRange;
        playerData.moveSpeed += equipment.totalMoveSpeed;
        playerData.criticalChance += equipment.totalCriticalChance;
        playerData.criticalDamage += equipment.totalCriticalDamage;
        playerData.penetration += equipment.totalPenetration;
    }
}
