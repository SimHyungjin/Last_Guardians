using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;
using System.Collections;
public interface ITrapEffect
{
    public void Apply(BaseMonster target, TowerData towerData,bool bossImmunebuff);
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
    
    [SerializeField] private List<int> buffTowerIndex;
    [SerializeField] private List<ITrapEffect> trapEffectList;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;

    private Coroutine activeEffectCoroutine;
    private TowerData towerData;
    private TrapObjectState currentState;

    private Dictionary<Type, int> currentEffectSources = new();

    private float cooldownTime;
    private float creationTime;

    private float effectRadius = 0.5f;
    public bool bossImmunebuff = false;


    private float maxbuffRadius = 2.5f;

    private bool disable = false;  
    public  void Init(TowerData towerData)
    {
        this.towerData = towerData;
        cooldownTime = towerData.EffectDuration;
        creationTime = Time.time;
        
        buffTowerIndex = new List<int>();
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

        Collider2D[] blockHits = Physics2D.OverlapPointAll(pos, LayerMaskData.buildBlock);
        foreach (var hit in blockHits)
        {
            if (hit != null && hit.gameObject != gameObject)
            {
                ChageState(TrapObjectState.CantActive);
                return;
            }
        }

        Collider2D[] trapHits = Physics2D.OverlapPointAll(pos, LayerMaskData.trapObject);
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
        Collider2D hit = Physics2D.OverlapPoint(PostionArray(), LayerMaskData.buildBlock);
        return hit != null;
    }
    public Vector2 PostionArray()
    {
        return new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }


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
                        effect.Apply(target, towerData, bossImmunebuff);
                        break;
                    }
                }
                else
                {
                    foreach (var hit in hits)
                    {
                        BaseMonster monster = hit.GetComponent<BaseMonster>();
                        if (monster == null) continue;

                        effect.Apply(monster, towerData, bossImmunebuff);
                    }
                }
            }

            yield return new WaitForSeconds(applyInterval);
            elapsed += applyInterval;
        }

        ChageState(TrapObjectState.Cooldown);
    }


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
    public  void DestroyBuffTower()
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
    private void ScanBuffTower()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxbuffRadius, LayerMaskData.tower);

        foreach (var hit in hits)
        {
            BuffTower otherTower = hit.GetComponent<BuffTower>();
            if (otherTower != null && otherTower != this)
            {
                otherTower.ReApplyBuff();
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
        if(isActiveAndEnabled)TowerManager.Instance.StartCoroutine(TowerManager.Instance.NotifyTrapObjectNextFrame(transform.position));
    }
}
