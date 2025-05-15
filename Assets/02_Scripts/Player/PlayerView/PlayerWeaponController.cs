using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WeaponAnimatorEntry
{
    public AttackType attackType;
    public AnimatorOverrideController animatorController;
}

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private SpriteRenderer effectRenderer;
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] private List<WeaponAnimatorEntry> weaponAnimators = new();

    public Action<Vector2> attackActionEnter;
    public Action attackAction;
    public Action attackActionExit;

    public Vector2 targetPos;
    private Color baseColor;

    private Dictionary<AttackType, AnimatorOverrideController> weaponAnimatorMap = new();

    private void Awake()
    {
        foreach (var entry in weaponAnimators)
        {
            if (!weaponAnimatorMap.ContainsKey(entry.attackType))
                weaponAnimatorMap.Add(entry.attackType, entry.animatorController);
        }
    }

    public string SetWeapon(AttackType attackType, Sprite icon)
    {
        if (icon != null)
            weaponRenderer.sprite = icon;

        weaponAnimator.runtimeAnimatorController =
            weaponAnimatorMap.TryGetValue(attackType, out var controller)
            ? controller
            : weaponAnimatorMap.GetValueOrDefault(AttackType.Melee);

        ApplyWeaponPivotByType(attackType);

        string label;
        if (icon == null)
            label = "Default";
        else if (attackType == AttackType.Melee)
            label = "Melee";
        else if (attackType == AttackType.Ranged)
            label = "Bow";
        else
            label = "Magic";
        return label;
    }

    private void ApplyWeaponPivotByType(AttackType type)
    {
        if (type == AttackType.Ranged)
        {
            SetWeaponTransform(new Vector3(-0.07f, -0.12f, 0), -170f);
        }
        else if (type == AttackType.Area)
        {
            SetWeaponTransform(new Vector3(-0.05f, -0.15f, 0), -80f);
        }
        else
        {
            SetWeaponTransform(new Vector3(0.05f, -0.2f, 0), 100f);
        }
    }

    private void SetWeaponTransform(Vector3 localPosition, float zRotation)
    {
        weaponRenderer.transform.localPosition = localPosition;
        weaponRenderer.transform.localRotation = Quaternion.Euler(0, 0, zRotation);
    }

    public void CallAttackEnter(Vector2 targetPos)
    {
        weaponAnimator.SetTrigger("IsAttack");
        this.targetPos = targetPos;
    }


    public void CallAtack()
    {
        attackAction?.Invoke();
    }

    public void CallAttackEnd()
    {
        attackActionExit?.Invoke();
    }
}
