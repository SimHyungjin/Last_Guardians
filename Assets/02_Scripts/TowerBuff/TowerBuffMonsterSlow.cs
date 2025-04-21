public class TowerBuffMonsterSlow : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData data)
    {

    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data)
    {
    }
    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {
        monster.ApplySlowdown(data.EffectValue, 0.1f);
    }
}
