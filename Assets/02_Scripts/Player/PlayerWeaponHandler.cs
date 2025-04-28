using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public struct WeaponSpriteEntry
{
    public AttackType attackType;
    public Sprite sprite;
}
public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] private List<WeaponSpriteEntry> weaponSprites = new();

    private Dictionary<AttackType, Sprite> weaponSpriteMap = new();

    private void Awake()
    {
        weaponRenderer = GetComponent<SpriteRenderer>();

        foreach (var weaponSprite in weaponSprites)
        {
            if (!weaponSpriteMap.ContainsKey(weaponSprite.attackType))
            {
                weaponSpriteMap.Add(weaponSprite.attackType, weaponSprite.sprite);
            }
        }
    }

    public void SetWeapon(AttackType attackType)
    {
        if (weaponSpriteMap.TryGetValue(attackType, out var sprite)) weaponRenderer.sprite = sprite;
        else weaponRenderer.sprite = weaponSpriteMap.GetValueOrDefault(AttackType.Melee);
    }
}
