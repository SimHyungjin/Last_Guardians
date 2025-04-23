using UnityEngine;

public class TowerBuffAddProjectileComponent : ITowerBuff
{
    public void ApplyBuffToTower(BaseTower tower, TowerData data, EnvironmentEffect environmentEffect)
    {
        if (tower is AttackTower attackTower)
        {
            attackTower.AddEffect(data.TowerIndex,environmentEffect);
            Debug.Log($"{attackTower.towerData.TowerName},{data.TowerIndex}");
        }
        else if (tower is BuffTower buffTower)
        {
            buffTower.AddEffect(data.TowerIndex,environmentEffect);
            Debug.Log($"{buffTower.towerData.TowerName},{data.TowerIndex}");
        }
    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data, EnvironmentEffect environmentEffect)
    {
        if (trap is TrapObject trapObject)
        {
            trapObject.AddEffect(data.TowerIndex);
        }
    }
    public void ApplyDebuff(BaseMonster monster, TowerData data, EnvironmentEffect environmentEffect)
    {

    }
}
