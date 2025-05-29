using System.Collections.Generic;
using UnityEngine;

public class ProjectileChainAttackEffect : MonoBehaviour, IEffect
{
    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect, bool bossImmune)
    {
        //주변에 스플레시 대미지
        if (towerData.EffectTargetCount == 0)
        {
            //if (towerData.EffectTarget == EffectTarget.All)
            //{
            //    target.TakeDamage(adaptedTowerData.attackPower * adaptedTowerData.effectValue);
            //}
            //else
            //{
            //    BossMonster bossMonster = target.GetComponent<BossMonster>();
            //    if (bossMonster != null)
            //    {
            //        bossMonster.TakeDamage(adaptedTowerData.attackPower * adaptedTowerData.effectValue);
            //    }
            //}
        }
        //피격시 피격위치에서 추가탄환 발사
        else
        {
            TowerData ownerTowerData = this.gameObject.GetComponent<ProjectileBase>().GetTowerData();
            if (ownerTowerData.EffectTarget == EffectTarget.BossOnly)
            {
                if (target.MonsterData.MonsterType == MonType.Boss)
                    ChainShot(target, ownerTowerData, adaptedTowerData, environmentEffect);
            }
            else
            {
                ChainShot(target, ownerTowerData, adaptedTowerData, environmentEffect);
            }
        }
    }

    public void Apply(BaseMonster target, TowerData towerData, AdaptedAttackTowerData adaptedTowerData, float chance, EnvironmentEffect environmentEffect, bool bossImmune)
    {

    }

    /// <summary>
    /// 추가탄환 발사로직
    /// 기존의 Projectile에서 추가로 발사하되 ChainAttackRffect를 제거하고 발사(무한증식 방지)
    /// 블래스터도 구현되어있으나 사용하지 않아서 주석처리
    /// </summary>
    /// <param name="target"></param>
    /// <param name="ownerTowerData"></param>
    /// <param name="adaptedTowerData"></param>
    private void ChainShot(BaseMonster target, TowerData ownerTowerData, AdaptedAttackTowerData adaptedTowerData, EnvironmentEffect environmentEffect)
    {
        float angleStep = 10f;
        int additionalCount = ownerTowerData.EffectTargetCount;
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
            List<int> effectsubself = new List<int>();
            effectsubself = adaptedTowerData.buffTowerIndex;
            for (int j = 0; j < effectsubself.Count; j++)
            {
                if (effectsubself[j] == ownerTowerData.TowerIndex)
                {
                    effectsubself.Remove(ownerTowerData.TowerIndex);
                    continue;
                }
            }
            switch (ownerTowerData.ProjectileType)
            {
                case ProjectileType.Magic:
                    MagicProjectile magicprojectile = PoolManager.Instance.Spawn(TowerManager.Instance.projectileFactory.ReturnPrefabs<MagicProjectile>(ownerTowerData));
                    magicprojectile.OriginTarget = target;
                    magicprojectile.transform.position = spawnPosition;
                    magicprojectile.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));
                    magicprojectile.Init(ownerTowerData, adaptedTowerData, effectsubself, environmentEffect);
                    TowerManager.Instance.projectileFactory.AddAllEffects(magicprojectile, effectsubself);
                    magicprojectile.Launch(origin + dir * 10f);
                    break;
                case ProjectileType.Blast:
                    BlastProjectile blastProjectile = PoolManager.Instance.Spawn(TowerManager.Instance.projectileFactory.ReturnPrefabs<BlastProjectile>(ownerTowerData));

                    blastProjectile.OriginTarget = target;
                    blastProjectile.transform.position = spawnPosition;
                    blastProjectile.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, dir));
                    blastProjectile.Init(ownerTowerData, adaptedTowerData, effectsubself, environmentEffect);
                    TowerManager.Instance.projectileFactory.AddAllEffects(blastProjectile, effectsubself);
                    blastProjectile.Launch(origin + dir * 0.3f);
                    break;
            }
        }
    }
}
