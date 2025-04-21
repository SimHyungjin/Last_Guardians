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
            TowerData ownerTowerData = this.gameObject.GetComponent<ProjectileBase>().GetTowerData();
            float angleStep = 10f;
            int additionalCount = towerData.EffectTargetCount - 1;

            Vector2 origin = transform.position;
            Vector2 forward = transform.right;

            int half = additionalCount / 2;
            for (int i = 0; i < additionalCount; i++)
            {
                Debug.Log($"[ChainAttack] {i} / {additionalCount}");
                float spawnOffset = 0.5f;

                int index = i - half;
                if (additionalCount % 2 == 0 && index >= 0) index += 1;

                float angle = angleStep * index;
                Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;

                Vector2 spawnPosition = origin + dir * spawnOffset;
                Vector2 targetPosition = origin + dir * 10f;

                switch (ownerTowerData.ProjectileType)
                {
                    case ProjectileType.Magic:
                        MagicProjectile magicprojectile = PoolManager.Instance.Spawn(TowerManager.Instance.projectileFactory.ReturnPrefabs<MagicProjectile>(ownerTowerData));
                        magicprojectile.OriginTarget = target;
                        magicprojectile.transform.position = spawnPosition;
                        magicprojectile.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));
                        magicprojectile.Init(ownerTowerData,null, null);
                        magicprojectile.Launch(origin + dir * 10f);
                        break;
                    case ProjectileType.Blast:
                        BlastProjectile blastProjectile = TowerManager.Instance.projectileFactory.ReturnPrefabs<BlastProjectile>(ownerTowerData);
                        blastProjectile.OriginTarget = target;
                        blastProjectile.transform.position = spawnPosition;
                        blastProjectile.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));
                        blastProjectile.Init(ownerTowerData, null, null);
                        blastProjectile.Launch(origin + dir * 10f);
                        break;
                }
            }
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, float chance)
    {

    }
}
