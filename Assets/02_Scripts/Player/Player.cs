using System.Collections;
using UnityEngine;

public class Player
{
    public PlayerData playerData {  get; private set; }

    public void Init()
    {
        if(playerData == null) playerData = new();
    }

    public void SetStatus()
    {
        var equipment = GameManager.Instance.stats;
        if (playerData == null || equipment == null) return;

        playerData.attackType = equipment.attackType;
        playerData.attackPower = playerData.baseAttackPower + equipment.attack;
        playerData.attackSpeed = playerData.baseAttackSpeed - equipment.attackSpeed;
        playerData.attackRange = playerData.baseAttackRange + equipment.attackRange;
        playerData.moveSpeed = playerData.baseMoveSpeed + equipment.moveSpeed;
        playerData.criticalChance = playerData.baseCriticalChance + equipment.criticalChance;
        playerData.criticalDamage = playerData.baseCriticalDamage + equipment.criticalDamage;
        playerData.penetration = playerData.basePenetration + equipment.penetration;

        InGameManager.Instance.playerManager.playerController.weaponHandler.SetWeapon(playerData.attackType);
    }
}
