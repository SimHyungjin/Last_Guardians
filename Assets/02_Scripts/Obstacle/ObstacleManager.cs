using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : Singleton<ObstacleManager>
{
    [SerializeField] private ObstacleData[] obstacleDatas;

    public ObstacleData GetData(ObstacleType type, Season season, Weather weather)
    {
        foreach (var data in obstacleDatas)
        {
            if (data.season == season && data.weather == weather && data.obstacleType == type)
                return data;
        }

        return null;
    }
}
