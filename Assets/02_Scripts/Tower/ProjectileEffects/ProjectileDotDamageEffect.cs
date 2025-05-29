using UnityEngine;

public class ProjectileDotDamageEffect : MonoBehaviour, IEffect
{
    ///////////======================도트데미지 이펙트====================================/////////////////////
    float addObtacleValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect, bool bossImmune)
    {
        IsFire(environmentEffect);
        if (target.MonsterData.MonsterType == MonType.Boss && towerData.EffectTarget == EffectTarget.BossOnly)
        {
            target.DotDamage(adaptedTowerData.effectValue, adaptedTowerData.effectDuration * addObtacleValue);
        }
        else if (Utils.ShouldApplyEffect(target, towerData, bossImmune))
        {
            target.DotDamage(adaptedTowerData.effectValue, adaptedTowerData.effectDuration * addObtacleValue);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect, bool bossImmune)
    {
        IsFire(environmentEffect);
        if (target.MonsterData.MonsterType == MonType.Boss && towerData.EffectTarget == EffectTarget.BossOnly)
        {
            if (Random.value < chance)
            {
                target.DotDamage(adaptedTowerData.effectValue, adaptedTowerData.effectDuration * addObtacleValue);
            }
        }
        else if (Utils.ShouldApplyEffect(target, towerData, bossImmune))
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
            }
            else
            {
                addObtacleValue = 1f;
            }
        }
        else
        {
            if (environmentEffect.IsFireBoosted())
            {
                addObtacleValue = 1.25f;
            }
            else
            {
                addObtacleValue = 1f;
            }
        }
    }
}
