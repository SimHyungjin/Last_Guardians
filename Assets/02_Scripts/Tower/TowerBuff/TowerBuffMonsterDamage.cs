public class TowerBuffMonsterDamage : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {

    }
    public void ApplyBuffToTrap(TrapObject trap, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {

    }
    public void ApplyDebuff(BaseMonster monster, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
        monster.TakeDamage(data.effectValue);
    }
}
