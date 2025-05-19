using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDefReducEffect : MonoBehaviour, IEffect
{
    ///////////=====================���������� ���°���=====================================/////////////////////
    float addWeatherValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            isFog();
            target.ApplyReducionDef(adaptedTowerData.effectValue + addWeatherValue, adaptedTowerData.effectDuration);
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            isFog();
            if (Random.value < chance)
            {
                target.ApplyReducionDef(adaptedTowerData.effectValue + addWeatherValue, adaptedTowerData.effectDuration);
            }
        }
    }
    /// <summary>
    /// �Ȱ������϶� �߰� ���� ����
    /// </summary>
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
