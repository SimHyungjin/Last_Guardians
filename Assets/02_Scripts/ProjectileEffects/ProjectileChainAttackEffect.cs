using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileChainAttackEffect : MonoBehaviour,IEffect
{
    public void Apply(BaseMonster target, TowerData towerData)
    {
        if (towerData.EffectTargetCount == 0)
        {
            if (towerData.EffectTarget == EffectTarget.All)
            {
                target.TakeDamage(towerData.AttackPower * towerData.EffectValue);
            }
            else
            {
                BossMonster bossMonster = target.GetComponent<BossMonster>();
                if (bossMonster != null)
                {
                    bossMonster.TakeDamage(towerData.AttackPower * towerData.EffectValue);
                }
            }
        }
        else
        {
            float angleStep = 10f;
            int additionalCount = towerData.EffectTargetCount - 1;

            Vector2 origin = transform.position;
            Vector2 forward = transform.right;

            int half = additionalCount / 2;

            for (int i = 0; i < additionalCount; i++)
            {
                float spawnOffset = 0.5f;

                int index = i - half;
                if (additionalCount % 2 == 0 && index >= 0) index += 1;

                float angle = angleStep * index;
                Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;

                Vector2 spawnPosition = origin + dir * spawnOffset;
                Vector2 targetPosition = origin + dir * 10f;

                var projectile = PoolManager.Instance.Spawn(GetComponent<ProjectileBase>());
                projectile.OriginTarget = target;
                projectile.transform.position = spawnPosition;
                projectile.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));
                projectile.Init(towerData, null);
                projectile.Launch(origin + dir * 10f);
                Debug.Log($"Spawned projectile at angle: {angle} degrees");
            }

        }
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {

    }
}
