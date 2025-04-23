public class TowerBuffMonsterDotDamage : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData data, EnvironmentEffect environmentEffect)
    {

    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data, EnvironmentEffect environmentEffect)
    {

    }
    public void ApplyDebuff(BaseMonster monster, TowerData data, EnvironmentEffect environmentEffect)
    {
        monster.DotDamage(data.EffectValue, data.EffectDuration);
    }
}
