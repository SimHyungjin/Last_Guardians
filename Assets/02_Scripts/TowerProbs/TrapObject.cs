using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.PlayerSettings;
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
    private ITrapEffect trapEffect;
    [SerializeField] private LayerMask buildBlockMask;
    private TrapObjectState currentState = TrapObjectState.CantActive;
    private float cooldownTime;
    public void Init(TowerData towerData)
    {

        cooldownTime = towerData.EffectDuration;

        if (effectTypeMap.TryGetValue(towerData.SpecialEffect, out var effectType))
        {
                var effect = gameObject.AddComponent(effectType) as ITrapEffect;
                trapEffect = effect;
        }
        else
        {
            Debug.LogWarning($"[TrapObject] ��ϵ��� ���� SpecialEffect: {towerData.SpecialEffect}");
        }
        buildBlockMask=LayerMask.GetMask("Tower", "Obstacle");
        
        if (IsAnyObjectOnTile(new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y))))
        {
            Debug.Log("Ÿ�Ͽ� �浹ü����");
        }
        else
        {
            Debug.Log("Ÿ�Ͽ� �浹ü����");
        }
        //Collider2D hit = Physics2D.OverlapPoint(worldPos, LayerMask.GetMask("Tower", "Obstacle"));�̸� ��ġ���ϰ�
        //Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }


    private static readonly Dictionary<SpecialEffect, Type> effectTypeMap = new()
    {
        { SpecialEffect.DotDamage, typeof(TrapObjectDotDamageEffect) },//�̱���
        { SpecialEffect.Slow, typeof(TrapObjectSlowEffect) },//�̱���
        { SpecialEffect.BossDebuff, typeof(TrapObjectBossDebuffEffect) },//�̱���
        { SpecialEffect.Knockback, typeof(TrapObjectKnockbackEffect) },//�̱���
    };

    public bool IsAnyObjectOnTile(Vector2 tilePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, buildBlockMask);
        return hit != null;
    }

    public void ChageState(TrapObjectState trapObjectState)
    {
        currentState = trapObjectState;
    }
}
