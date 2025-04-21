public class TowerBuffAttackPower : ITowerBuff
{
    public void ApplyBuff(BaseTower tower, TowerData data)
    {
        if (tower is AttackTower attackTower)
        {
            AttackTower buffedattackTower = tower.GetComponent<AttackTower>();
            if(buffedattackTower!=null) buffedattackTower.AttackPowerBuff(data.TowerIndex);
        }
    }

    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {

    }
}
