public class TowerBuffMonsterDotDamage : ITowerBuff
{
    public void ApplyBuff(BaseTower tower, TowerData data)
    {

    }
    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {
        monster.DotDamage(data.EffectValue, 0.2f);
    }
}
