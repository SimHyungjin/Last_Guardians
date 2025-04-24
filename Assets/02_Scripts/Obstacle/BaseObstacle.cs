using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BaseObstacle : MonoBehaviour
{
    [SerializeField] private ObstacleData obstacle;
    private SpriteRenderer spriteRenderer;
    private NavMeshObstacle navMeshObstacle;

    [SerializeField] private ObstacleType obstacleType;
    [SerializeField] private Season season;
    [SerializeField] private Weather weather;

    private List<GameObject> zones = new();
    [SerializeField] private GameObject zonePrefab;

    //private void Start()
    //{
    //    Init(ObstacleType.Water);
    //    Init(Season.summer);
    //}

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    public void Init(ObstacleType _obstacleType)
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //navMeshObstacle = GetComponent<NavMeshObstacle>();
        obstacleType = _obstacleType;
        season = Season.Default;
        weather = Weather.Default;
        ChangeLayer();
    }

    public void Init(Season _season)
    {
        season = _season;
        ChangeObstacleData(FindObstacle());
    }

    public void Init(Weather _weather)
    {
        weather = _weather;
        ChangeObstacleData(FindObstacle());
    }

    public void ChangeObstacleData(ObstacleData data)
    {
        obstacle = data;
        //spriteRenderer.sprite = data.sprite;

        ChangeNavActive();
        SetZone();
    }

    private ObstacleData FindObstacle()
    {
        var list = ObstacleManager.Instance.GetAllObstacleData()
        .Where(data => data.obstacleType == obstacleType)
        .ToList();

        return list.FirstOrDefault(data =>
                   data.season == season && data.weather == weather) ??
               list.FirstOrDefault(data =>
                   data.season == season && data.weather == Weather.Default) ??
               list.FirstOrDefault(data =>
                   data.season == Season.Default && data.weather == weather) ??
               list.FirstOrDefault(data =>
                   data.season == Season.Default && data.weather == Weather.Default);
    }

    private void ChangeLayer()
    {
        switch(obstacleType)
        {
            case ObstacleType.Rock:
            case ObstacleType.Ruin:
                gameObject.layer = LayerMaskData.ObstacleMask;
                break;
            case ObstacleType.Platform:
                gameObject.layer = LayerMaskData.PlatformMask;
                break;
            case ObstacleType.Trap:
                gameObject.layer = LayerMaskData.TrapObject;
                break;
            default:
                gameObject.layer = LayerMaskData.PlantedObstacleMask;
                break;
        }
    }

    private void ChangeNavActive()
    {
        if(obstacle == null) return;
        navMeshObstacle.enabled = !(obstacle.passbyPlayer && obstacle.passbyMonster);
    }

    private Vector2[] offsets = new Vector2[]
    {
        new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0),
    };

    private void SetZone()
    {
        zones.Clear();
        for (int i = 0; i < offsets.Length; i++)
        {
            Vector2 worldPos = (Vector2)transform.position + offsets[i];

            GameObject zoneObj = Instantiate(zonePrefab, transform);
            zoneObj.SetActive(true);
            zoneObj.transform.position = worldPos;
            var zone = zoneObj.GetComponent<PlantedEffect>();
            zone.Init(obstacle.obstacleType);
            zones.Add(zoneObj);
        }
    }

    private void EffectToPlayer(PlayerController controller)
    {
        if (obstacle == null) return;
        var handler = controller.playerBuffHandler;
        switch (obstacle.obstacleEffect_Player)
        {
            case ObstacleEffect.Speed:
                handler.ApplyBuff(new PlayerBuffMoveSpeed(obstacle.obstacleEffect_PlayerValue, Mathf.Infinity, true));
                break;

            case ObstacleEffect.Stun:
                handler.ApplyBuff(new PlayerBuffStun(obstacle.obstacleEffect_PlayerValue, controller));
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var controller))
        {
            EffectToPlayer(controller);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var controller))
        {
            controller.playerBuffHandler.ClearAllBuffs();
        }
    }

}