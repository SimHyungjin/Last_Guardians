public class TowerBuffBossImmuneDelBuff : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData data, EnvironmentEffect environmentEffect)
    {
        if (tower is AttackTower attackTower)
        {
            AttackTower buffedattackTower = tower.GetComponent<AttackTower>();
            if (buffedattackTower != null) buffedattackTower.BossImmuneBuff();
        }
    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data, EnvironmentEffect environmentEffect)
    {
        if (trap is TrapObject trapObject)
        {
            TrapObject buffedTrap = trap.GetComponent<TrapObject>();
            if (buffedTrap != null) buffedTrap.BossImmuneBuff();
        }
    }
    public void ApplyDebuff(BaseMonster monster, TowerData data, EnvironmentEffect environmentEffect)
    {

    }
}