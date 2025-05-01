using Unity.VisualScripting;

public class TowerBuffMonsterSlow : ITowerBuff
{

    private float addObstacleValue = 0f;
    public void ApplyBuffToTower(BaseTower tower, TowerData datae, EnvironmentEffect environmentEffect)
    {

    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data, EnvironmentEffect environmentEffect)
    {
    }
    public void ApplyDebuff(BaseMonster monster, TowerData data, EnvironmentEffect environmentEffect)
    {
        IsWater(environmentEffect);
        monster.ApplySlowdown(data.EffectValue+ addObstacleValue, data.EffectDuration);
    }
    private void IsWater(EnvironmentEffect environmentEffect)
    {
        if (EnviromentManager.Instance.Season == Season.winter)
        {
            addObstacleValue = 0f;
        }
        else
        {
            if (EnviromentManager.Instance.WeatherState.GetCurrentState() is RainWeather)
                if (environmentEffect.IsWaterBoosted())
                {
                    addObstacleValue = 0.6f;
                }
                else
                {
                    addObstacleValue = 0f;
                }
            else
            {
                if (environmentEffect.IsWaterBoosted())
                {
                    addObstacleValue = 0.3f;
                }
                else
                {
                    addObstacleValue = 0f;
                }
            }
        }
    }
}
