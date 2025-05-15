public class TowerBuffBossImmuneDelBuff : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
        if (tower is AttackTower attackTower)
        {
            AttackTower buffedattackTower = tower.GetComponent<AttackTower>();
            if (buffedattackTower != null) buffedattackTower.BossImmuneBuff();
        }
    }
    public void ApplyBuffToTrap(TrapObject trap, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
        if (trap is TrapObject trapObject)
        {
            TrapObject buffedTrap = trap.GetComponent<TrapObject>();
            if (buffedTrap != null) buffedTrap.BossImmuneBuff();
        }
    }
    public void ApplyDebuff(BaseMonster monster, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {

    }
}