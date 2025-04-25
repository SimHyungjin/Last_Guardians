using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSlowEffect : MonoBehaviour, IEffect
{
    private float addObstacleValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        IsWater(environmentEffect);
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            target.ApplySlowdown(towerData.EffectValue+ addObstacleValue, towerData.EffectDuration);
            Debug.Log($"기본 슬로우 {towerData.EffectValue}, {towerData.EffectDuration}");
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
                Debug.Log($"찬스 슬로우 {towerData.EffectValue}, {towerData.EffectDuration}");
            }
        }
    }

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
