using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDotDamageEffect : MonoBehaviour, IEffect
{
    float addObtacleValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        IsFire(environmentEffect);
        if (target.MonsterData.MonsterType == MonType.Boss && towerData.EffectTarget == EffectTarget.BossOnly)
        {
            target.DotDamage(towerData.EffectValue, towerData.EffectDuration * addObtacleValue);
        }
        else if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            target.DotDamage(towerData.EffectValue, towerData.EffectDuration * addObtacleValue);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect)
    {
        IsFire(environmentEffect);
        if (target.MonsterData.MonsterType == MonType.Boss && towerData.EffectTarget == EffectTarget.BossOnly)
        {
            if (Random.value < chance)
            {
                target.DotDamage(towerData.EffectValue, towerData.EffectDuration * addObtacleValue);
            }
        }
        else if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            if (Random.value < chance)
            {
                target.DotDamage(towerData.EffectValue, towerData.EffectDuration * addObtacleValue);
            }
        }
    }
    public void IsFire(EnvironmentEffect environmentEffect)
    {
        if (EnviromentManager.Instance.WeatherState.GetCurrentState() is RainWeather)
        {
            if (environmentEffect.IsFireBoosted())
            {
                addObtacleValue = 1.20f;
                Debug.Log("비오는날 화염이 적용되었습니다.");
            }
            else
            {
                addObtacleValue = 1f;
                Debug.Log("화염이 적용되지 않았습니다.");
            }
        }
        else
        {
            if (environmentEffect.IsFireBoosted())
            {
                addObtacleValue = 1.25f;
                Debug.Log("화염이 적용되었습니다.");
            }
            else
            {
                addObtacleValue = 1f;
                Debug.Log("화염이 적용되지 않았습니다.");
            }
        }
    }
}
