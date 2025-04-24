using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseObstacle : MonoBehaviour
{
    private ObstacleData obstacle;
    private SpriteRenderer spriteRenderer;
    private NavMeshObstacle navMeshObstacle;

    private List<GameObject> zones = new();
    [SerializeField] private GameObject zonePrefab;

    private void Start()
    {
        //테스트용
        Init(ObstacleManager.Instance.GetData(ObstacleType.Rock, Season.All, Weather.All));
    }

    public void Init(ObstacleData data)
    {
        if(navMeshObstacle == null) navMeshObstacle = GetComponent<NavMeshObstacle>();
        obstacle = data;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.sprite;

        ChangeLayer();
        ChangeNavActive();
        SetZone();
    }

    private void ChangeLayer()
    {
        if (obstacle == null) return;
        switch(obstacle.obstacleType)
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
        for (int i = 0; i < offsets.Length; i++)
        {
            Vector2 worldPos = (Vector2)transform.position + offsets[i];

            GameObject zoneObj = Instantiate(zonePrefab, transform);
            zoneObj.SetActive(true);
            zoneObj.transform.position = worldPos;
            var zone = zoneObj.GetComponent<PlantedEffect>();
            zone.Init(obstacle.obstacleType);
            if (zoneObj != null) zones.Add(zoneObj);
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