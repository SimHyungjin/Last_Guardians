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
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Collider2D col;
    private TowerData towerData;
    private TrapObjectState currentState;
    private float cooldownTime;
    public  void Init(TowerData towerData)
    {
        this.towerData = towerData;
        cooldownTime = towerData.EffectDuration;
        trapEffectIndex = new List<int>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        if (towerData.SpecialEffect != SpecialEffect.None)
        {
            trapEffectIndex.Add(towerData.TowerIndex);
        }

        buildBlockMask =LayerMask.GetMask("Tower", "Obstacle","TrapObject");
        
        if (IsAnyObjectOnTile())
        {
            Debug.Log("Ÿ�Ͽ� �浹ü����");
            UnActive(TrapObjectState.CantActive);
        }
        else
        {
            Debug.Log("Ÿ�Ͽ� �浹ü����");
            ChageState(TrapObjectState.Ready);
        }
    }
    private  void Update()
    {
        if (currentState == TrapObjectState.Cooldown)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0)
            {
                OnActive();
                cooldownTime = towerData.EffectDuration;
            }
        }
    }

    private static readonly Dictionary<SpecialEffect, Type> effectTypeMap = new()
    {
        { SpecialEffect.DotDamage, typeof(TrapObjectDotDamageEffect) },//�̱���
        { SpecialEffect.Slow, typeof(TrapObjectSlowEffect) },//�̱���
        { SpecialEffect.BossDebuff, typeof(TrapObjectBossDebuffEffect) },//�̱���
        { SpecialEffect.Knockback, typeof(TrapObjectKnockbackEffect) },//�̱���
    };

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
                Debug.Log("ȿ���ѷǰ� ��Ÿ�ӵ���");
                UnActive(TrapObjectState.Cooldown);
            }
        }
    }

    public void OnActive()
    {
        ChageState(TrapObjectState.Ready);
        sr.enabled = true;
        col.enabled = true;
    }

    public void UnActive(TrapObjectState trapObjectState)
    {
        ChageState(trapObjectState);
        sr.enabled = false;
        col.enabled = false;
    }

}
