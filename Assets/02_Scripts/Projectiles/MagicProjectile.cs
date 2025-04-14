using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicProjectile : ProjectileBase
{
    public override void Init(TowerData _towerData)
    {
        base.Init(_towerData);
        Debug.Log($"{towerData.ElementType}+{towerData.ElementType}");
        string path= $"Assets/string path/Sprite/ProjectileImage/{towerData.ElementType}{towerData.ProjectileType}";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        GetComponent<SpriteRenderer>().sprite= sprite;
    }

    protected override void ProjectileMove()
    {
        Debug.Log("Move");
        rb.velocity = direction * speed;
    }

}
