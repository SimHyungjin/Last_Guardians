using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour
{
    private ObstacleData obstacle;
    private SpriteRenderer spriteRenderer;

    private List<GameObject> zones = new();
    [SerializeField] private GameObject zonePrefab;

    private void Start()
    {
        Init(ObstacleManager.Instance.GetData(Season.Spring, Weather.All, ObstacleType.Water));
    }
    public void Init(ObstacleData data)
    {
        obstacle = data;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.sprite;

        ChangeLayer();
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
            default:
                gameObject.layer = LayerMaskData.PlantedObstacleMask;
                break;
        }
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
            var zone =zoneObj.GetComponent<PlantedEffect>();
            zone.Init(obstacle.obstacleType);
            if (zoneObj != null) zones.Add(zoneObj);
        }
    }
}