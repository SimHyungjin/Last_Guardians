using UnityEngine;

public class TrapObjectSlowEffect : MonoBehaviour, ITrapEffect
{
    ///////////==========================슬로우 이펙트================================/////////////////////

    private float addObstacleValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTrapObjectData adaptedTrapObjectData, bool bossImmunebuff, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, bossImmunebuff))
        {
            IsWater(environmentEffect);
            target.ApplySlowdown(adaptedTrapObjectData.effectValue + addObstacleValue, adaptedTrapObjectData.effectDuration);
        }
    }

    /// <summary>
    /// 계절에 따라 물속성 적용
    /// </summary>
    /// <param name="environmentEffect"></param>
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

