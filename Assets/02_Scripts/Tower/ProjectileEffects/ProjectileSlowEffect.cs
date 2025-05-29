using UnityEngine;

public class ProjectileSlowEffect : MonoBehaviour, IEffect
{
    ///////////==========================슬로우 이펙트================================/////////////////////
    private float addObstacleValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect, bool bossImmune)
    {
        IsWater(environmentEffect);
        if (Utils.ShouldApplyEffect(target, towerData, bossImmune))
        {
            target.ApplySlowdown(adaptedTowerData.effectValue + addObstacleValue, adaptedTowerData.effectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect,bool bossImmune)
    {
        IsWater(environmentEffect);
        if (Utils.ShouldApplyEffect(target, towerData, bossImmune))
        {
            if (Random.value < chance)
            {
                target.ApplySlowdown(towerData.EffectValue + addObstacleValue, towerData.EffectDuration);
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
