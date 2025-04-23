using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour
{
    private ObstacleData obstacle;
    private SpriteRenderer spriteRenderer;

    public void Init(ObstacleData data)
    {
        obstacle = data;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.sprite;
        ChangeLayer();
    }

    private void ChangeLayer()
    {
        if (obstacle == null) return;
        if (obstacle.towerPlace) gameObject.layer = LayerMask.GetMask("Obstacle");
        
    }
}




//private void ChangeLayer()
//{
//    if (obstacle == null) return;
//    if (!obstacle.passbyPlayer && !obstacle.passbyMonster) gameObject.layer = LayerMask.GetMask("Obstacle");
//    else if (!obstacle.passbyMonster && obstacle.passbyMonster) return;
//    else if (obstacle.passbyMonster && !obstacle.passbyMonster) return;
//    else if (obstacle.passbyMonster && obstacle.passbyPlayer) return;
//}
