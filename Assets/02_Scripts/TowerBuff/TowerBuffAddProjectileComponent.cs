using UnityEngine;

public class TowerBuffAddProjectileComponent : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData data)
    {
        if (tower is AttackTower attackTower)
        {
            attackTower.AddEffect(data.TowerIndex);
            Debug.Log($"{ attackTower.towerData.TowerName},{data.TowerIndex}");
        }
    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data)
    {
        if (trap is TrapObject trapObject)
        {
            trapObject.AddEffect(data.TowerIndex);
        }
    }
    public void ApplyDebuff(BaseMonster monster, TowerData data)
    {

    }
}
