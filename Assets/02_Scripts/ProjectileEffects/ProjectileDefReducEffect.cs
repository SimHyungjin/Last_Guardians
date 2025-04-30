using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDefReducEffect : MonoBehaviour, IEffect
{
    ///////////=====================보스미적용 방어력감소=====================================/////////////////////
    float addWeatherValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            isFog();
            target.ApplyReducionDef(towerData.EffectValue + addWeatherValue, towerData.EffectDuration);
            Debug.Log($"기본 방깍 {towerData.EffectValue}, {towerData.EffectDuration}");
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            isFog();
            if (Random.value < chance)
            {
                target.ApplyReducionDef(towerData.EffectValue + addWeatherValue, towerData.EffectDuration);
                Debug.Log($"찬스 방깍 {towerData.EffectValue}, {towerData.EffectDuration}");
            }
        }
    }
    /// <summary>
    /// 안개날씨일때 추가 방어력 감소
    /// </summary>
    public void isFog()
    {
        if (EnviromentManager.Instance.WeatherState.GetCurrentState() is FogWeather)
        {
            addWeatherValue = 0.15f;
            Debug.Log("안개라서 방깍 추가적용중");
        }
        else
        {
            addWeatherValue = 0f;
        }
    }
}
