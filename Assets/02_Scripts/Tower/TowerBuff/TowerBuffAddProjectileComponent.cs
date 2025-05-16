using UnityEngine;

public class TowerBuffAddProjectileComponent : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
        if (tower is AttackTower attackTower)
        {
            attackTower.AddEffect(data.towerIndex,environmentEffect);
        }
        else if (tower is BuffTower buffTower)
        {
            buffTower.AddEffect(data.towerIndex, environmentEffect);
        }
    }
    public void ApplyBuffToTrap(TrapObject trap, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
        if (trap is TrapObject trapObject)
        {
            trapObject.AddEffect(data.towerIndex);
        }
    }
    public void ApplyDebuff(BaseMonster monster, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {

    }
}
