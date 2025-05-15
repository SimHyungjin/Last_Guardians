public class TowerBuffAttackPower : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
        if (tower is AttackTower attackTower)
        {
            AttackTower buffedattackTower = tower.GetComponent<AttackTower>();
            if(buffedattackTower!=null) buffedattackTower.AttackPowerBuff(data.effectValue);
        }
    }
    public void ApplyBuffToTrap(TrapObject trap, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {

    }

    public void ApplyDebuff(BaseMonster monster, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {

    }
}
