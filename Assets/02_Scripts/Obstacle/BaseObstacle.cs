using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class BaseObstacle : MonoBehaviour
{
    [SerializeField] private ObstacleData obstacle;
    public ObstacleData ObstacleData => obstacle;
    private SpriteRenderer spriteRenderer;
    private NavMeshObstacle navMeshObstacle;

    [SerializeField] private ObstacleType obstacleType;
    [SerializeField] private Season season;
    [SerializeField] private Weather weather;
    [SerializeField] private Sprite sprite;

    private List<GameObject> zones = new();
    [SerializeField] private GameObject zonePrefab;

    private readonly float effectInterval = 1f;
    private Dictionary<Collider2D, float> effectTimers = new();

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

    private void OnDestroy()
    {
        DestroyZone();
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

        if (spriteRenderer != null && data.sprite != null)
        {
            spriteRenderer.sprite = data.sprite;  // ← 여기!
        }

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
        DestroyZone();
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
                handler.ApplyBuff(new PlayerBuffMoveSpeed(obstacle.obstacleEffect_PlayerValue, 0.2f, true));
                break;

            case ObstacleEffect.Stun:
                handler.ApplyBuff(new PlayerBuffStun(obstacle.obstacleEffect_PlayerValue, controller));
                break;
        }
    }

    private void EffectToMonster(BaseMonster baseMonster)
    {
        if(obstacle== null) return;

        switch (obstacle.obstacleEffect_Monster)
        {
            case ObstacleEffect.Speed:
                baseMonster.ApplySlowdown(0.7f, 0.2f);
                break;

            case ObstacleEffect.Dotdamage:
                baseMonster.DotDamage(obstacle.obstacleEffect_MonsterValue, 0.5f);
                break;
            case ObstacleEffect.Dead:
                baseMonster.TakeDamage(9999999);
                break;
        }
    }

    private void DestroyZone()
    {
        foreach (var zone in zones)
        {
            Destroy(zone);
        }
        zones.Clear();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!effectTimers.ContainsKey(collision))
        {
            effectTimers[collision] = 0f;
        }

        effectTimers[collision] += Time.deltaTime;

        if (effectTimers[collision] >= effectInterval)
        {
            if (collision.TryGetComponent<PlayerController>(out var controller))
            {
                EffectToPlayer(controller);
            }

            if (collision.TryGetComponent<BaseMonster>(out var baseMonster))
            {
                EffectToMonster(baseMonster);
            }

            effectTimers[collision] = 0f; // 타이머 초기화
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (effectTimers.ContainsKey(collision))
        {
            effectTimers.Remove(collision); // 나가면 제거
        }
    }
}