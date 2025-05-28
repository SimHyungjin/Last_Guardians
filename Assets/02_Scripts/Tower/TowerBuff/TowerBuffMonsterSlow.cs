public class TowerBuffMonsterSlow : ITowerBuff
{

    private float addObstacleValue = 0f;
    public void ApplyBuffToTower(BaseTower tower, AdaptedBuffTowerData datae, EnvironmentEffect environmentEffect)
    {

    }
    public void ApplyBuffToTrap(TrapObject trap, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
    }
    public void ApplyDebuff(BaseMonster monster, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
        IsWater(environmentEffect);
        monster.ApplySlowdown(data.effectValue + addObstacleValue, data.effectDuration);
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
