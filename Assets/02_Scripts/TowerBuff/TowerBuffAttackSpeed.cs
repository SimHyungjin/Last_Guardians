public class TowerBuffAttackSpeed : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData data)
    {
        if (tower is AttackTower attackTower)
        {
            AttackTower buffedattackTower = tower.GetComponent<AttackTower>();
            if (buffedattackTower != null) buffedattackTower.AttackSpeedBuff(data.TowerIndex);
        }
    }

    public void ApplyBuffToTrap(TrapObject trap, TowerData data)
    {
    }
    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {

    }
}
