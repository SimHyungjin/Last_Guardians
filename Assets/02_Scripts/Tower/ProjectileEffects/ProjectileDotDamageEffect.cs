using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDotDamageEffect : MonoBehaviour, IEffect
{
    ///////////======================도트데미지 이펙트====================================/////////////////////
    float addObtacleValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
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

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect)
    {
        IsFire(environmentEffect);
        if (target.MonsterData.MonsterType == MonType.Boss && towerData.EffectTarget == EffectTarget.BossOnly)
        {
            if (Random.value < chance)
            {
                target.DotDamage(adaptedTowerData.effectValue, adaptedTowerData.effectDuration * addObtacleValue);
            }
        }
        else if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            if (Random.value < chance)
            {
                target.DotDamage(adaptedTowerData.effectValue, adaptedTowerData.effectDuration * addObtacleValue);
            }
        }
    }

    /// <summary>
    /// 불 장애물 근처에 있을때 적용되는 로직
    /// </summary>
    /// <param name="environmentEffect"></param>
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
