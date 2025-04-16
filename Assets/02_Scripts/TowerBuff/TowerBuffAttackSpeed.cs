public class TowerBuffAttackSpeed : ITowerBuff
{
    public void ApplyBuff(BaseTower tower, TowerData data)
    {
        if (tower is AttackTower attackTower)
        {
            attackTower.AttackSpeedBuff(data.EffectValue);
        }
    }

    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {

    }
}
