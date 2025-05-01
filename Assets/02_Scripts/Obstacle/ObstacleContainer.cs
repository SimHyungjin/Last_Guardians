using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class ObstacleContainer
{
    [SerializeField] private ObstacleData[] obstacleDatas;

    public ObstacleContainer()
    {
        obstacleDatas = Resources.LoadAll<ObstacleData>("SO/Obstacle");
    }

    public ObstacleData GetData(ObstacleType type, Season season, Weather weather)
    {
        foreach (var data in obstacleDatas)
        {
            if (data.season == season && data.weather == weather && data.obstacleType == type)
                return data;
        }

        return null;
    }

    public ReadOnlyArray<ObstacleData> GetAllObstacleData() => obstacleDatas;
}
