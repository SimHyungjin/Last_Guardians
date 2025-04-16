public class TowerBuffMonsterDamage : ITowerBuff
{
    public void ApplyBuff(BaseTower tower, TowerData data)
    {

    }
    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {
        monster.TakeDamage(data.EffectValue);
    }
}
