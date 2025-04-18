using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;
public interface ITrapEffect
{
    void Apply(BaseMonster target, TowerData towerData);
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
    [SerializeField] private LayerMask buildBlockMask;
    [SerializeField] private LayerMask TrapObjectMask;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;
    private TowerData towerData;
    private TrapObjectState currentState;
    private float cooldownTime;

    private float creationTime;

    public  void Init(TowerData towerData)
    {
        
        this.towerData = towerData;
        cooldownTime = towerData.EffectDuration;
        creationTime = Time.time;
        
        trapEffectIndex = new List<int>();
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
        if (towerData.SpecialEffect != SpecialEffect.None)
        {
            trapEffectIndex.Add(towerData.TowerIndex);
        }
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
        { SpecialEffect.BossDebuff, typeof(TrapObjectBossDebuffEffect) },//미구현
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
                Debug.Log("타일에 타워/장애물 있음");
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
                Debug.Log("다른 트랩이 이미 우선권 가짐");
                ChageState(TrapObjectState.CantActive);
                return;
            }
        }

        Debug.Log("트랩 설치 가능");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState != TrapObjectState.Ready) return;
        if (collision.CompareTag("Monster"))
        {
            BaseMonster monster = collision.GetComponent<BaseMonster>();
            if (monster != null)
            {
                //foreach (int effectIndex in trapEffectIndex)
                //{
                //    SpecialEffect effect = (SpecialEffect)effectIndex;
                //    Type effectType = effectTypeMap[effect];
                //    ITrapEffect trapEffect = (ITrapEffect)Activator.CreateInstance(effectType);
                //    trapEffect.Apply(monster, TowerManager.Instance.TowerData[effectIndex]);
                //}
                Debug.Log("효과뿌렷고 쿨타임돈다");
                //ChangeState(TrapObjectState.Triggered);
                ChageState(TrapObjectState.Cooldown);
            }
        }
    }
    private void StartEffect()
    {
        ChageState(TrapObjectState.Cooldown);
    }
    public void OnActive()
    {
        sr.enabled = true;
    }

    public void UnActive()
    {
        sr.enabled = false;
    }

    private void OnDestroy()
    {
        TowerManager.Instance.StartCoroutine(TowerManager.Instance.NotifyTrapObjectNextFrame(transform.position));
    }
}
