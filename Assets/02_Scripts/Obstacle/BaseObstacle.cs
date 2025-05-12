using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;  

/// <summary>
/// BaseObstacle는 장애물의 기본 클래스입니다.
/// ObstacleData를 사용하여 룰타일 또는 스프라이트로 표시합니다.
/// </summary>
public class BaseObstacle : MonoBehaviour
{
    [SerializeField] private ObstacleData obstacle;
    public ObstacleData ObstacleData => obstacle;

    [SerializeField] private ObstacleType obstacleType;
    [SerializeField] private Season season;
    [SerializeField] private Weather weather;
    [SerializeField] private GameObject zonePrefab;

    private SpriteRenderer spriteRenderer;
    private NavMeshObstacle navMeshObstacle;
    private Tilemap obstacleTilemap;   // ← 캐싱된 타일맵 참조
    private readonly float effectInterval = 0.2f;
    private List<GameObject> zones = new();
    private Dictionary<Collider2D, float> effectTimers = new();

    private Dictionary<PlayerHandler, IPlayerBuff<PlayerData>> playerEffects = new();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

        // InGameManager 에서 캐싱된 타일맵 가져오기
        obstacleTilemap = InGameManager.Instance.ObstacleTilemap;
    }

    /// <summary>
    /// 최초 타입만 설정할 때 호출
    /// </summary>
    public void Init(ObstacleType _obstacleType)
    {
        obstacleType = _obstacleType;
        season = Season.Default;
        weather = Weather.Default;
        ChangeLayer();
    }

    private void OnDestroy()
    {
        DestroyZone();
    }

    /// <summary>
    /// 시즌/날씨 초기화 시 호출
    /// </summary>
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

    /// <summary>
    /// ObstacleData가 바뀔 때 룰타일 또는 스프라이트로 표시
    /// </summary>
    public void ChangeObstacleData(ObstacleData data)
    {
        obstacle = data;

        // 1) 룰타일이 세팅되어 있고, 타일맵이 유효하면
        if (data.ruleTile != null && obstacleTilemap != null)
        {
            Vector3Int cellPos = obstacleTilemap.WorldToCell(transform.position);
            obstacleTilemap.SetTile(cellPos, data.ruleTile);

            // 스프라이트/네비 활성화 끄기
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (navMeshObstacle != null) navMeshObstacle.enabled = false;
            return;
        }

        // 2) 룰타일이 없으면 기존 스프라이트 방식
        if (spriteRenderer != null && data.sprite != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = data.sprite;
        }
        ChangeNavActive();
        SetZone();
    }

    private ObstacleData FindObstacle()
    {
        var list = InGameManager.Instance.obstacleContainer
                       .GetAllObstacleData()
                       .Where(d => d.obstacleType == obstacleType)
                       .ToList();

        return list.FirstOrDefault(d => d.season == season && d.weather == weather)
            ?? list.FirstOrDefault(d => d.season == season && d.weather == Weather.Default)
            ?? list.FirstOrDefault(d => d.season == Season.Default && d.weather == weather)
            ?? list.FirstOrDefault(d => d.season == Season.Default && d.weather == Weather.Default);
    }

    private void ChangeLayer()
    {
        switch (obstacleType)
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
        if (obstacle == null) return;
        navMeshObstacle.enabled = !(obstacle.passbyPlayer && obstacle.passbyMonster);
    }

    private void SetZone()
    {
        DestroyZone();
        Vector2[] offsets = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

        foreach (var offset in offsets)
        {
            Vector2 worldPos = (Vector2)transform.position + offset;
            GameObject zoneObj = Instantiate(zonePrefab, transform);
            zoneObj.transform.position = worldPos;
            zoneObj.SetActive(true);

            var zone = zoneObj.GetComponent<PlantedEffect>();
            zone.Init(obstacle.obstacleType);
            zones.Add(zoneObj);
        }
    }

    private void DebuffToPlayer(PlayerHandler controller)
    {
        if (controller == null) return;

        IPlayerBuff<PlayerData> effect = obstacle.obstacleEffect_Player switch
        {
            ObstacleEffect.Speed => new PlayerBuffMoveSpeed(obstacle.obstacleEffect_PlayerValue, float.MaxValue, true),
            ObstacleEffect.Stun => new PlayerBuffStun(obstacle.obstacleEffect_PlayerValue, controller),
            _ => null
        };

        if (effect == null) return;

        playerEffects[controller] = effect;
        controller.playerObstacleDebuff.EnterZone(effect);
    }


    private void DestroyZone()
    {
        foreach (var z in zones) Destroy(z);
        zones.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHandler>(out var controller))
            DebuffToPlayer(controller);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!effectTimers.ContainsKey(collision))
            effectTimers[collision] = 0f;

        effectTimers[collision] += Time.deltaTime;

        if (effectTimers[collision] <= effectInterval)
            return;

        if (!collision.TryGetComponent<BaseMonster>(out var monster))
            return;

        Season season = EnviromentManager.Instance.Season;
        effectTimers[collision] = 0f;

        switch (ObstacleData.obstacleType)
        {
            case ObstacleType.Mud:
                monster.ApplySlowdown(season == Season.spring ? 0.15f : 0.3f, 0.2f);
                break;

            case ObstacleType.Water:
                float slowdown = season switch
                {
                    Season.spring => 0.2f,
                    Season.summer => 0.25f,
                    Season.autumn => 0.3f,
                    Season.winter => 0.35f,
                    _ => 0.2f
                };
                monster.ApplySlowdown(slowdown, 0.2f);
                break;

            case ObstacleType.Fire:
                float damage = season switch
                {
                    Season.summer => 6f,
                    Season.winter => 4f,
                    _ => 5f
                };
                monster.DotDamage(damage, 0.2f);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (effectTimers.ContainsKey(collision))
            effectTimers.Remove(collision);

        if (collision.TryGetComponent<PlayerHandler>(out var controller) && playerEffects.TryGetValue(controller, out var effect))
        {
            controller.playerObstacleDebuff.ExitZone(effect);
            playerEffects.Remove(controller);
        }
    }
}
