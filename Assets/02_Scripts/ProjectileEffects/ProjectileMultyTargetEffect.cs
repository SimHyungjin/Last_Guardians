using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ProjectileFactory;

public class ProjectileMultyTargetEffect : MonoBehaviour, IEffect
{

    public void Apply(BaseMonster target, TowerData towerData)
    {
        float angleStep = 10f;
        int additionalCount = towerData.EffectTargetCount - 1;

        Vector2 origin = transform.position;
        Vector2 forward = transform.right;

        int half = additionalCount / 2;


        for (int i = 0; i < additionalCount; i++)
        {
            int index = i - half;
            if (additionalCount % 2 == 0 && index >= 0) index += 1;

            float angle = angleStep * index;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;
            var projectile = PoolManager.Instance.Spawn(GetComponent<ProjectileBase>());
            projectile.transform.position = origin;
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
            projectile.Init(towerData);
            projectile.Launch(origin + dir * 10f);
            Debug.Log($"Spawned projectile at angle: {angle} degrees");
        }
    }
    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {
    }
}

