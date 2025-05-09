using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
public interface ITrapEffect
{
    public void Apply(BaseMonster target, TowerData towerData,bool bossImmunebuff, EnvironmentEffect environmentEffect);
}
public enum TrapObjectState
{
    CantActive, 
    Ready,
    Triggered,
    Cooldown
}
public class TrapObject : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<int> buffTowerIndex;
    [SerializeField] private List<ITrapEffect> trapEffectList;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;
    public EnvironmentEffect environmentEffect;
    private Coroutine activeEffectCoroutine;
    private Coroutine checkOverlapCoroutine;
    private TowerData towerData;
    private Animator animator;

    private TrapObjectState currentState;

    private Dictionary<Type, int> currentEffectSources = new();

    private float cooldownTime;
    private float creationTime;

    private float effectRadius = 1f;
    public bool bossImmunebuff = false;


    private float maxbuffRadius = 2.5f;

    private bool disable = false;  
    public  void Init(TowerData towerData)
    {
        this.towerData = towerData;
        cooldownTime = towerData.EffectDuration;
        creationTime = Time.time;
        environmentEffect = new EnvironmentEffect();
        buffTowerIndex = new List<int>();
        trapEffectList= new List<ITrapEffect>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        Utils.GetColor(towerData, GetComponent<SpriteRenderer>());
        buffTowerIndex.Add(towerData.TowerIndex);
        AddTrapEffect(towerData.TowerIndex);
        CanPlant();
    }
    private  void Update()
    {
        if (currentState == TrapObjectState.Cooldown)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0)
            {
                ChageState(TrapObjectState.Ready);
                cooldownTime = towerData.EffectDuration;
            }
        }
    }

    /// <summary>
    /// Ʈ���� ����Ǵ� ����Ʈ ����Ʈ
    /// </summary>
    private static readonly Dictionary<SpecialEffect, Type> effectTypeMap = new()
    {
        { SpecialEffect.DotDamage, typeof(TrapObjectDotDamageEffect) },
        { SpecialEffect.Slow, typeof(TrapObjectSlowEffect) },
        { SpecialEffect.Silence, typeof(TrapObjectSilenceEffect) },
        { SpecialEffect.Knockback, typeof(TrapObjectKnockbackEffect) },
    };

    /// <summary>
    /// ��ġ ���ɿ��� �Ǻ�
    /// </summary>
    public void CanPlant()
    {
        Vector2 pos = PostionArray();
        Collider2D[] blockHits = Physics2D.OverlapPointAll(pos, LayerMaskData.buildBlock);
        foreach (var hit in blockHits)
        {
            if (hit != null && hit.gameObject != gameObject)
            {
                ChageState(TrapObjectState.CantActive);
                return;
            }
        }

        Collider2D[] plantedObstcles = Physics2D.OverlapPointAll(pos, LayerMaskData.plantedObstacle);
        foreach (var hit in plantedObstcles)
        {
            if (hit != null && hit.gameObject != gameObject)
            {
                ChageState(TrapObjectState.CantActive);
                return;
            }
        }

        Collider2D[] floorHits = Physics2D.OverlapPointAll(pos, LayerMaskData.obstacleZone);
        environmentEffect.isNearWater = false;
        environmentEffect.isNearFire = false;
        foreach (var hit in floorHits)
        {
            PlantedEffect plantedEffect = hit.GetComponent<PlantedEffect>();
            switch (plantedEffect.obstacleType)
            {
                case ObstacleType.Water:
                    Debug.Log("��ġ��ġ���� ������");
                    environmentEffect.isNearWater = true;
                    break;
                case ObstacleType.Fire:
                    Debug.Log("��ġ��ġ���� ������");
                    environmentEffect.isNearFire = true;
                    break;

            }
        }

        Collider2D[] trapHits = Physics2D.OverlapPointAll(pos, LayerMaskData.trapObject);
        checkOverlapCoroutine=StartCoroutine(CheckTrapOverlap(trapHits));
    }

    /// <summary>
    /// Ʈ�� ������Ʈ�� ��ġ���� üũ�ϴ� �ڷ�ƾ ���� ���� ������ Ʈ��Ȱ��ȭ�ȴ�.
    /// </summary>
    /// <param name="trapHits"></param>
    /// <returns></returns>
    private IEnumerator CheckTrapOverlap(Collider2D[] trapHits)
    {
        yield return null;
        if (this == null) yield break;
        foreach (var hit in trapHits)
        {
            if (hit == null || hit.gameObject == this.gameObject) continue;
            TrapObject other = hit.GetComponent<TrapObject>();
            if (other != null && other.creationTime < this.creationTime && other.currentState != TrapObjectState.CantActive)
            {
                Debug.Log($"�浹ü����{other}");
                ChageState(TrapObjectState.CantActive);
                checkOverlapCoroutine= null;
                yield break;
            }
        }
        checkOverlapCoroutine = null;
        ChageState(TrapObjectState.Ready);
    }


    public bool IsAnyObjectOnTile()
    {
        Collider2D hit = Physics2D.OverlapPoint(PostionArray(), LayerMaskData.buildBlock);
        return hit != null;
    }

    public Vector2 PostionArray()
    {
        return new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    /// <summary>
    /// Ʈ���� ���Ͱ� ������� ȣ��ȴ�.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState != TrapObjectState.Ready) return;
        if (disable) return;
        if (collision.CompareTag("Monster"))
        {
            BaseMonster monster = collision.GetComponent<BaseMonster>();
            if (monster != null)
            {
                ChageState(TrapObjectState.Triggered);
                if (activeEffectCoroutine != null)
                    StopCoroutine(activeEffectCoroutine);
                activeEffectCoroutine = StartCoroutine(ApplyEffectsToArea(monster));
            }
        }
    }

    /// <summary>
    /// Ʈ������Ʈ ����
    /// Ÿ���� ��Ƽ���� �̱����� �����Ͽ� �����ð����� ����Ʈ���� �� ��Ÿ������ ��ȯ 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator ApplyEffectsToArea(BaseMonster target)
    {
        float elapsed = 0f;
        float applyInterval = 0.1f;

        while (elapsed < towerData.EffectDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effectRadius, LayerMaskData.monster);

            foreach (var effect in trapEffectList)
            {
                if (towerData.EffectTarget == EffectTarget.Single)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.GetComponent<BaseMonster>() == target) 
                        effect.Apply(target, towerData, bossImmunebuff, environmentEffect);
                        break;
                    }
                }
                else
                {
                    foreach (var hit in hits)
                    {
                        BaseMonster monster = hit.GetComponent<BaseMonster>();
                        if (monster == null) continue;

                        effect.Apply(monster, towerData, bossImmunebuff, environmentEffect);
                    }
                }
            }

            yield return new WaitForSeconds(applyInterval);
            elapsed += applyInterval;
        }
        activeEffectCoroutine = null;
        animator.SetTrigger("TrapObjectDespawn");
        yield return new WaitForSeconds(0.1f);
        animator.enabled = false;
        ChageState(TrapObjectState.Cooldown);
    }

    /// <summary>
    /// Ʈ���� ����Ǵ� ����Ʈ �߰�
    /// </summary>
    /// <param name="index"></param>
    public void AddTrapEffect(int index)
    {
        TowerData data = TowerManager.Instance.GetTowerData(index);
        SpecialEffect effectKey = data.SpecialEffect;

        if (!effectTypeMap.TryGetValue(effectKey, out var effectType))
        {
            return;
        }

        if (TryGetComponent(effectType, out var existingComp))
        {
            int existingIndex = currentEffectSources.TryGetValue(effectType, out var prevIndex) ? prevIndex : -1;
            if (existingIndex != -1)
            {
                float existingValue = TowerManager.Instance.GetTowerData(existingIndex).EffectValue;
                if (existingValue >= data.EffectValue)
                {
                    return;
                }

                Destroy(existingComp as Component);
                currentEffectSources.Remove(effectType);

                trapEffectList.RemoveAll(e => e?.GetType() == effectType);
            }
        }
        var newEffect = gameObject.AddComponent(effectType) as ITrapEffect;
        if (newEffect != null)
        {
            trapEffectList.Add(newEffect);
            currentEffectSources[effectType] = index;
        }
    }


    public void BossImmuneBuff()
    {
        bossImmunebuff = true;
    }
    public void RemoveBossImmuneBuff()
    {
        bossImmunebuff = false;
    }

    public void AddEffect(int targetIndex)
    {
        bool found = false;

        for (int i = 0; i < buffTowerIndex.Count; i++)
        {
            if (buffTowerIndex[i] == targetIndex)
            {
                var existing = TowerManager.Instance.GetTowerData(buffTowerIndex[i]);
                if (existing.EffectValue < TowerManager.Instance.GetTowerData(targetIndex).EffectValue)
                {
                    buffTowerIndex[i] = targetIndex;
                    AddTrapEffect(targetIndex);
                }
                found = true;
                break;
            }
        }

        if (!found)
        {
            buffTowerIndex.Add(targetIndex);
            AddTrapEffect(targetIndex);
        }
    }

    public void OnDisabled()
    {
        disable = true;
        Invoke("OnEnabled", 3.5f);
    }
    public void OnEnabled()
    {
        disable = false;
    }

    ///////////==========================�����ʱ�ȭ �Լ���================================/////////////////////

    public void DestroyBuffTower()
    {
        ClearAllbuff();
        ScanBuffTower();
    }

    private void ClearAllbuff()
    {
       
        buffTowerIndex.Clear();
        trapEffectList.Clear();
        RemoveBossImmuneBuff();
        buffTowerIndex.Add(towerData.TowerIndex);
        AddTrapEffect(towerData.TowerIndex);
    }

    /// <summary>
    /// ����Ÿ���� ��ĵ�Ͽ� ������ �������Ѵ�.
    /// </summary>
    private void ScanBuffTower()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxbuffRadius, LayerMaskData.tower);

        foreach (var hit in hits)
        {
            BuffTower otherTower = hit.GetComponent<BuffTower>();
            if (otherTower != null)
            {
                otherTower.ReApplyBuff();
            }
        }
    }

    /// <summary>
    /// �ֺ��� ��ֹ����� ��ĵ�Ͽ� ������ �������Ѵ�.
    /// </summary>
    public void ScanPlantedObstacle()
    {
        environmentEffect.isNearWater = false;
        environmentEffect.isNearFire = false;

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position, LayerMaskData.obstacleZone);
        foreach (var hit in hits)
        {
            PlantedEffect plantedEffect = hit.GetComponent<PlantedEffect>();
            switch (plantedEffect.obstacleType)
            {
                case ObstacleType.Water:
                    Debug.Log("��ġ��ġ���� ������");
                    environmentEffect.isNearWater = true;
                    break;
                case ObstacleType.Fire:
                    Debug.Log("��ġ��ġ���� ������");
                    environmentEffect.isNearFire = true;
                    break;

            }
        }
    }
    public void OnActive()
    {
        sr.enabled = true;
    }

    public void UnActive()
    {
        sr.enabled = false;
    }

    public void ChageState(TrapObjectState trapObjectState)
    {
        currentState = trapObjectState;
        switch (trapObjectState)
        {
            case TrapObjectState.CantActive:
                animator.enabled = false;
                UnActive();
                break;
            case TrapObjectState.Ready:
                animator.enabled = true;
                OnActive();
                break;
            case TrapObjectState.Triggered:
                animator.SetTrigger("TrapObjectActive");
                break;
            case TrapObjectState.Cooldown:
                UnActive();
                break;
        }
    }


    private void OnDestroy()
    {
        if (checkOverlapCoroutine != null)
        {
            StopCoroutine(checkOverlapCoroutine);
            checkOverlapCoroutine = null;
        }
        if (activeEffectCoroutine != null)
        {
            StopCoroutine(activeEffectCoroutine);
            activeEffectCoroutine = null;
        }
        if (isActiveAndEnabled)TowerManager.Instance.StartCoroutine(TowerManager.Instance.NotifyTrapObjectNextFrame(transform.position));
    }
}
