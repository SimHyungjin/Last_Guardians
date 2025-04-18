using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;
using System.Collections;
public interface ITrapEffect
{
    public void Apply(BaseMonster target, TowerData towerData);
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
    
    [SerializeField] private List<int> trapEffectIndex;
    [SerializeField] private List<ITrapEffect> trapEffectList;
    [SerializeField] private LayerMask buildBlockMask;
    [SerializeField] private LayerMask TrapObjectMask;
    [SerializeField] private LayerMask MonsterMask;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;

    private Coroutine activeEffectCoroutine;
    private TowerData towerData;
    private TrapObjectState currentState;

    private Dictionary<Type, int> currentEffectSources = new();

    private float cooldownTime;
    private float creationTime;

    public  void Init(TowerData towerData)
    {
        
        this.towerData = towerData;
        cooldownTime = towerData.EffectDuration;
        creationTime = Time.time;
        
        trapEffectIndex = new List<int>();
        trapEffectList= new List<ITrapEffect>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        //테스트용 색깔놀이
        switch (towerData.ElementType)
        {
            case ElementType.Fire:
                sr.color = Color.red;
                break;
            case ElementType.Water:
                sr.color = Color.blue;
                break;
            case ElementType.Wind:
                sr.color = Color.cyan;
                break;
            case ElementType.Earth:
                sr.color = Color.green;
                break;
            case ElementType.Light:
                sr.color = Color.yellow;
                break;
            case ElementType.Dark:
                sr.color = Color.black;
                break;
            default:
                sr.color = Color.white;
                break;
        }
        trapEffectIndex.Add(towerData.TowerIndex);
        AddTrapEffect(towerData.TowerIndex);
        /////////////////////////////////////////////////////////////////오브젝트 설치시 주변에 버프타워있는지 검사이후 그 버프 가져오기 필요
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

    private static readonly Dictionary<SpecialEffect, Type> effectTypeMap = new()
    {
        { SpecialEffect.DotDamage, typeof(TrapObjectDotDamageEffect) },//미구현
        { SpecialEffect.Slow, typeof(TrapObjectSlowEffect) },//미구현
        { SpecialEffect.Silence, typeof(TrapObjectSilenceEffect) },//미구현
        { SpecialEffect.Knockback, typeof(TrapObjectKnockbackEffect) },//미구현
    };

    public void CanPlant()
    {
        Vector2 pos = PostionArray();

        Collider2D[] blockHits = Physics2D.OverlapPointAll(pos, buildBlockMask);
        foreach (var hit in blockHits)
        {
            if (hit != null && hit.gameObject != gameObject)
            {
                ChageState(TrapObjectState.CantActive);
                return;
            }
        }

        Collider2D[] trapHits = Physics2D.OverlapPointAll(pos, LayerMask.GetMask("TrapObject"));
        foreach (var hit in trapHits)
        {
            if (hit.gameObject == this.gameObject) continue;

            TrapObject other = hit.GetComponent<TrapObject>();
            if (other != null && other.creationTime < this.creationTime && other.currentState != TrapObjectState.CantActive)
            {
                ChageState(TrapObjectState.CantActive);
                return;
            }
        }
        ChageState(TrapObjectState.Ready);
    }

    public bool IsAnyObjectOnTile()
    {
        Collider2D hit = Physics2D.OverlapPoint(PostionArray(), buildBlockMask);
        return hit != null;
    }
    public Vector2 PostionArray()
    {
        return new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState != TrapObjectState.Ready) return;
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
    private IEnumerator ApplyEffectsToArea(BaseMonster target)
    {
        float elapsed = 0f;
        float applyInterval = 0.1f;

        while (elapsed < towerData.EffectDuration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f, MonsterMask);

            foreach (var effect in trapEffectList)
            {
                if (towerData.EffectTarget == EffectTarget.Single)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.GetComponent<BaseMonster>() == target) 
                        effect.Apply(target, towerData); ;
                        break;
                    }
                }
                else
                {
                    foreach (var hit in hits)
                    {
                        BaseMonster monster = hit.GetComponent<BaseMonster>();
                        if (monster == null) continue;

                        effect.Apply(monster, towerData);
                    }
                }
            }

            yield return new WaitForSeconds(applyInterval);
            elapsed += applyInterval;
        }

        ChageState(TrapObjectState.Cooldown);
    }

    //private IEnumerator ApplyEffectsToArea()
    //{
    //    float elapsed = 0f;
    //    float Interval = 0.1f;
    //    while (elapsed < towerData.EffectDuration)
    //    {
    //        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f, MonsterMask);

    //        foreach (var hit in hits)
    //        {
    //            BaseMonster monster = hit.GetComponent<BaseMonster>();
    //            if (monster == null) continue;

    //            foreach (var effect in trapEffectList)
    //            {
    //              effect.Apply(monster, towerData);
    //            }
    //        }

    //        yield return new WaitForSeconds(Interval);
    //        elapsed += Interval;
    //    }

    //    ChageState(TrapObjectState.Cooldown);
    //}
    ////단일대상
    //private IEnumerator ApplyEffectsOverTime(BaseMonster target)
    //{
    //    float elapsed = 0f;
    //    float interval = 0.1f;

    //    while (elapsed < towerData.EffectDuration)
    //    {
    //        foreach (var effect in trapEffectList)
    //        {
    //          effect.Apply(target, towerData);
    //        }

    //        yield return new WaitForSeconds(interval);
    //        elapsed += interval;
    //    }
    //    activeEffectCoroutine = null;

    //    ChageState(TrapObjectState.Cooldown);
    //}

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



    /// /////////////////////////////////////////////////////////////////////////////////////////////////주변에 버프타워 로직
    public void AddBuff(int BuffTowerIndex)
    {

    }
    public void SubtractBuff()
    {

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
                UnActive();
                break;
            case TrapObjectState.Ready:
                OnActive();
                break;
            case TrapObjectState.Triggered:
                break;
            case TrapObjectState.Cooldown:
                UnActive();
                break;
        }
    }

    private void OnDestroy()
    {
        TowerManager.Instance.StartCoroutine(TowerManager.Instance.NotifyTrapObjectNextFrame(transform.position));
    }
}
