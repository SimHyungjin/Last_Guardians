public class TowerBuffMonsterSlow : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData datae, EnvironmentEffect environmentEffect)
    {

    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data, EnvironmentEffect environmentEffect)
    {
    }
    public void ApplyDebuff(BaseMonster monster, TowerData data, EnvironmentEffect environmentEffect)
    {
        monster.ApplySlowdown(data.EffectValue, data.EffectDuration);
    }
}
