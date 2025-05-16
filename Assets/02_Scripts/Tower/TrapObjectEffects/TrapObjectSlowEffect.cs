using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObjectSlowEffect : MonoBehaviour,ITrapEffect
{
    ///////////==========================���ο� ����Ʈ================================/////////////////////

    private float addObstacleValue = 0f;
    public void Apply(BaseMonster target, TowerData towerData,AdaptedTrapObjectData adaptedTrapObjectData, bool bossImmunebuff, EnvironmentEffect environmentEffect)
    {
        if (Utils.ShouldApplyEffect(target, towerData, bossImmunebuff))
        {
            IsWater(environmentEffect);
            target.ApplySlowdown(adaptedTrapObjectData.effectValue + addObstacleValue, adaptedTrapObjectData.effectDuration);
            Debug.Log($"[Apply] {target.name} ���ο� ����Ʈ ����� Ÿ���ε��� : {towerData.TowerName},{towerData.EffectValue + addObstacleValue},{towerData.EffectDuration}");
        }
    }

    /// <summary>
    /// ������ ���� ���Ӽ� ����
    /// </summary>
    /// <param name="environmentEffect"></param>
    private void IsWater(EnvironmentEffect environmentEffect)
    {
        if (EnviromentManager.Instance.Season == Season.winter)
        {
            addObstacleValue = 0f;
            Debug.Log("�ܿ��̶� ���Ӽ��� ������� �ʾҽ��ϴ�.");
        }
        else
        {
            if (EnviromentManager.Instance.WeatherState.GetCurrentState() is RainWeather)
                if (environmentEffect.IsWaterBoosted())
                {
                    addObstacleValue = 0.6f;
                    Debug.Log("����³� ���Ӽ��� ����Ǿ����ϴ�.");
                }
                else
                {
                    addObstacleValue = 0f;
                    Debug.Log("���Ӽ��� ������� �ʾҽ��ϴ�.");
                }
            else
            {
                if (environmentEffect.IsWaterBoosted())
                {
                    addObstacleValue = 0.3f;
                    Debug.Log("���Ӽ��� ����Ǿ����ϴ�.");
                }
                else
                {
                    addObstacleValue = 0f;
                    Debug.Log("���Ӽ��� ������� �ʾҽ��ϴ�.");
                }
            }
        }
    }
}

