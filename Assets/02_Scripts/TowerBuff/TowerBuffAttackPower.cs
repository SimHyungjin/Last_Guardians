public class TowerBuffAttackPower : ITowerBuff
{
    public void ApplyBuff(BaseTower tower, TowerData data)
    {
        if (tower is AttackTower attackTower)
        {
            attackTower.AddEffect(data.TowerIndex);
        }
    }

    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {

    }
}
