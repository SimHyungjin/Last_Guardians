public class TowerBuffAttackPower : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData data, EnvironmentEffect environmentEffect)
    {
        if (tower is AttackTower attackTower)
        {
            AttackTower buffedattackTower = tower.GetComponent<AttackTower>();
            if(buffedattackTower!=null) buffedattackTower.AttackPowerBuff(data.EffectValue);
        }
    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data, EnvironmentEffect environmentEffect)
    {

    }

    public void ApplyDebuff(BaseMonster monster, TowerData data, EnvironmentEffect environmentEffect)
    {

    }
}
