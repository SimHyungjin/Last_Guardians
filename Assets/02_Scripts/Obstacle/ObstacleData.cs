using UnityEngine;

public enum ObstacleType
{
    Rock, Water, Mud, Fire, Ruin, Platform, Trap,
    None
}

public enum Weather
{
    Snow, Drought,
    Default, All,
}

public enum ObstacleEffect
{
    Dotdamage, Speed, Stun, Dead,
    None
}

public enum TowerBuff
{
    Speed, Range, DotdamageTime,
    None
}
public enum TargetTower
{
    Single,
    All, None
}
[CreateAssetMenu(fileName = "NewObstacle", menuName = "Data/Obstacle Data")]
public class ObstacleData : ScriptableObject
{
    public int obstacleIndex;
    public ObstacleType obstacleType;
    public Season season;
    public Weather weather;
    public string obstacleName;
    public bool passbyPlayer;
    public bool passbyMonster;
    public bool towerPlace;

    public ObstacleEffect obstacleEffect_Player;
    public float obstacleEffect_PlayerValue;
    public ObstacleEffect obstacleEffect_Monster;
    public float obstacleEffect_MonsterValue;

    public TowerBuff towerBuff;
    public TargetTower targetTower;
    public float towerBuffValue;

    public Sprite sprite;
    public string obstacleDescription;
}
