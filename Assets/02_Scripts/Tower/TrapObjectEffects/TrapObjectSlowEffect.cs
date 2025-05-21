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
            Debug.Log($"[Apply] {target.name} 슬로우 이펙트 적용됨 타워인덱스 : {towerData.TowerName},{towerData.EffectValue + addObstacleValue},{towerData.EffectDuration}");
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

