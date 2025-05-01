public class TowerBuffMonsterReducionDef : ITowerBuff
{
    float addWeatherValue = 0f;
    public void ApplyBuffToTower(BaseTower tower, TowerData data,EnvironmentEffect environmentEffect)
    {

    }
    public void ApplyBuffToTrap(TrapObject trap, TowerData data, EnvironmentEffect environmentEffect)
    {
    }
    public void ApplyDebuff(BaseMonster monster, TowerData data, EnvironmentEffect environmentEffect)
    {
        isFog();
        monster.ApplyReducionDef(data.EffectValue+ addWeatherValue, data.EffectValue);
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
