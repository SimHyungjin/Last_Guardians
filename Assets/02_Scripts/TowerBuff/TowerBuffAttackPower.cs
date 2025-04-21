public class TowerBuffAttackPower : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData data)
    {
        if (tower is AttackTower attackTower)
        {
            AttackTower buffedattackTower = tower.GetComponent<AttackTower>();
            if(buffedattackTower!=null) buffedattackTower.AttackPowerBuff(data.EffectValue);
        }
    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data)
    {

    }

    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {

    }
}
