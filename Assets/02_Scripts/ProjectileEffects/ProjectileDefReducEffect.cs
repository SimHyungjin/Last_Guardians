using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDefReducEffect : MonoBehaviour, IEffect
{
    ///////////=====================���������� ���°���=====================================/////////////////////
    float addWeatherValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData, AdaptedTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, adaptedTowerData.bossImmunebuff))
        {
            isFog();
            target.ApplyReducionDef(towerData.EffectValue + addWeatherValue, towerData.EffectDuration);
            Debug.Log($"�⺻ ��� {towerData.EffectValue}, {towerData.EffectDuration}");
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
                Debug.Log($"���� ��� {towerData.EffectValue}, {towerData.EffectDuration}");
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
            Debug.Log("�Ȱ��� ��� �߰�������");
        }
        else
        {
            addWeatherValue = 0f;
        }
    }
}
