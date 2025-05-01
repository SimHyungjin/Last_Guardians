using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSlowEffect : MonoBehaviour, IEffect
{
    ///////////==========================슬로우 이펙트================================/////////////////////
    private float addObstacleValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        IsWater(environmentEffect);
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            target.ApplySlowdown(towerData.EffectValue+ addObstacleValue, towerData.EffectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect)
    {
        IsWater(environmentEffect);
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            if (Random.value < chance)
            {
                target.ApplySlowdown(towerData.EffectValue+ addObstacleValue, towerData.EffectDuration);
            }
        }
    }

    /// <summary>
    /// 물 장애물 근처에있을때 적용되는 로직
    /// </summary>
    /// <param name="environmentEffect"></param>
    private void IsWater(EnvironmentEffect environmentEffect)
    {
        if (EnviromentManager.Instance.Season == Season.winter)
        {
            addObstacleValue = 0f;
            Debug.Log("겨울이라서 물속성이 적용되지 않았습니다.");
        }
        else
        {
            if (EnviromentManager.Instance.WeatherState.GetCurrentState() is RainWeather)
                if (environmentEffect.IsWaterBoosted())
                {
                    addObstacleValue = 0.6f;
                    Debug.Log("비오는날 물속성이 적용되었습니다.");
                }
                else
                {
                    addObstacleValue = 0f;
                    Debug.Log("물속성이 적용되지 않았습니다.");
                }
            else
            {
                if (environmentEffect.IsWaterBoosted())
                {
                    addObstacleValue = 0.3f;
                    Debug.Log("물속성이 적용되었습니다.");
                }
                else
                {
                    addObstacleValue = 0f;
                    Debug.Log("물속성이 적용되지 않았습니다.");
                }
            }
        }
    }
}
