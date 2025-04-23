using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantedEffect : MonoBehaviour
{
    public ObstacleType obstacleType;

    public void Init(ObstacleType _obstacleType)
    {
        obstacleType = _obstacleType;
        CanPos();
    }

    private Vector2 PostionArray()
    {
        return new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    private void CanPos()
    {
        Vector2 pos = PostionArray();
        Collider2D[] blockHits = Physics2D.OverlapPointAll(pos, LayerMaskData.buildBlock);
        foreach (var hit in blockHits)
        {
            if (hit != null && hit.gameObject != gameObject)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
