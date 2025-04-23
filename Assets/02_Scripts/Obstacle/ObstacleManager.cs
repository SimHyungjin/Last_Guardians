using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : Singleton<ObstacleManager>
{
    [SerializeField] private ObstacleData[] obstacleDatas;

    public ObstacleData GetData(Season season, Weather weather, ObstacleType type)
    {
        foreach (var data in obstacleDatas)
        {
            if (data.season == season && data.weather == weather && data.obstacleType == type)
                return data;
        }

        return null;
    }
}
