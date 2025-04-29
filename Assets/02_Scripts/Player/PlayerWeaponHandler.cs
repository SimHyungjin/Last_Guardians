using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponAnimatorEntry
{
    public AttackType attackType;
    public AnimatorOverrideController animatorController;
}

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] private List<WeaponAnimatorEntry> weaponAnimators = new();

    public Action<Vector2> attackActionEnter;
    public Action attackAction;
    public Action attackActionExit;

    private Dictionary<AttackType, AnimatorOverrideController> weaponAnimatorMap = new();
    private Vector3 defaultLocalPosition;

    private void Awake()
    {
        if (weaponAnimator == null) weaponAnimator = GetComponent<Animator>();
        if (weaponRenderer == null) weaponRenderer = GetComponent<SpriteRenderer>();

        defaultLocalPosition = weaponRenderer.transform.localPosition;

        foreach (var entry in weaponAnimators)
        {
            if (!weaponAnimatorMap.ContainsKey(entry.attackType))
                weaponAnimatorMap.Add(entry.attackType, entry.animatorController);
        }
    }

    public void SetWeapon(AttackType attackType)
    {
        if (weaponAnimatorMap.TryGetValue(attackType, out var controller))
            weaponAnimator.runtimeAnimatorController = controller;
        else
            weaponAnimator.runtimeAnimatorController = weaponAnimatorMap.GetValueOrDefault(AttackType.Melee);
    }

    public void CallAttackEnter(Vector2 targetPos)
    {
        weaponAnimator.SetTrigger("IsAttack");
        attackActionEnter?.Invoke(targetPos);
    }

    public void CallAtack()
    {
        attackAction?.Invoke();
    }

    public void CallAttackEnd()
    {
        attackActionExit?.Invoke();
    }

    public void SetFlip(bool flip)
    {
        weaponRenderer.flipX = flip;
        Vector3 pos = defaultLocalPosition;
        pos.x = flip ? -Mathf.Abs(pos.x) : Mathf.Abs(pos.x);
        weaponRenderer.transform.localPosition = pos;
    }
}
