public class TowerBuffMonsterReducionDef : ITowerBuff
{
    float addWeatherValue = 0f;
    public void ApplyBuffToTower(BaseTower tower, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {

    }
    public void ApplyBuffToTrap(TrapObject trap, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
    }
    public void ApplyDebuff(BaseMonster monster, AdaptedBuffTowerData data, EnvironmentEffect environmentEffect)
    {
        isFog();
        monster.ApplyReducionDef(data.effectValue + addWeatherValue, data.effectDuration);
    }
    public void isFog()
    {
        if (EnviromentManager.Instance.WeatherState.GetCurrentState() is FogWeather)
        {
            addWeatherValue = 0.15f;
        }
        else
        {
            addWeatherValue = 0f;
        }
    }
}
