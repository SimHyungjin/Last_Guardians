using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicProjectile : ProjectileBase
{
    public override void Init(TowerData _towerData)
    {
        base.Init(_towerData);
        string spritename = $"{towerData.ElementType}{towerData.ProjectileType}";
        Debug.Log(spritename);
        string path= $"Assets/91_Disign/Sprite/ProjectileImage/{spritename}.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        GetComponent<SpriteRenderer>().sprite= sprite;
    }

    protected override void ProjectileMove()
    {
        rb.velocity = direction * speed;
        Debug.Log($"Direction: {direction}, Speed: {speed}");
    }

}
