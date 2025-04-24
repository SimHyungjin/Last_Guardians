
using UnityEngine;

public static class LayerMaskData
{ 
    public static readonly LayerMask monster = LayerMask.GetMask("Monster");
    public static readonly LayerMask tower = LayerMask.GetMask("Tower");
    public static readonly LayerMask buildBlock= LayerMask.GetMask("Tower","Obstacle","Center");
    public static readonly LayerMask trapObject = LayerMask.GetMask("TrapObject");
    public static readonly LayerMask obstacle = LayerMask.GetMask("Obstacle");
    public static readonly LayerMask plantedObstacle = LayerMask.GetMask("PlantedObstacle");
    public static readonly LayerMask platform = LayerMask.GetMask("Platform");
    public static readonly LayerMask floor = LayerMask.GetMask("ObstacleZone", "PlantedObstacle", "TrapObject");
    public static readonly LayerMask obstacleZone = LayerMask.GetMask("ObstacleZone");
    public static readonly int ObstacleMask = LayerMask.NameToLayer("Obstacle");
    public static readonly int PlatformMask = LayerMask.NameToLayer("Platform");
    public static readonly int PlantedObstacleMask = LayerMask.NameToLayer("PlantedObstacle");
    public static readonly int TrapObject = LayerMask.NameToLayer("ObstacleTrap");
}
