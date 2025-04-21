public class TowerBuffMonsterReducionDef : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData data)
    {

    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data)
    {
    }
    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {
        monster.ApplyReducionDef(data.EffectValue, 0.2f);
    }
}
