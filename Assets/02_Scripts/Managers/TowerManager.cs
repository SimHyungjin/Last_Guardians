using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum InteractionState
{
    None,
    CardMoving,
    TowerMoving,
    Pause
}
public class TowerManager : Singleton<TowerManager>
{
    [Header("Tools")]
    public DeckHandler hand;
    public TowerCombinationData towerCombinationData;
    public Towerbuilder towerbuilder;
    public ProjectileFactory projectileFactory;

    [Header("Datas")]
    public GameObject TrapObjectPrefab;
    private Dictionary<int, TowerData> towerDataMap;
    public TowerUpgradeData towerUpgradeData;
    public TowerUpgradeValueData towerUpgradeValueData;

    [Header("Towers")]
    public List<BaseTower> Towers;
    public Sprite[] TowerIcons;

    public InteractionState CurrentState { get; private set; } = InteractionState.None;

    private void OnEnable()
    {
        towerDataMap = Resources.LoadAll<TowerData>("SO/Tower")
            .ToDictionary(td => td.TowerIndex, td => td);
    }


    public TowerData GetTowerData(int index)
    {
        return towerDataMap.TryGetValue(index, out var data) ? data : null;
    }

    public AdaptedBuffTowerData GetAdaptedBuffTowerData(int index)
    {
        TowerData towerdata =towerDataMap.TryGetValue(index, out var data) ? data : null;
        AdaptedBuffTowerData adaptedBuffTowerData = new AdaptedBuffTowerData(index,towerdata.EffectValue,towerdata.AttackRange,towerdata.EffectDuration);
        return adaptedBuffTowerData;
    }
    public AdaptedAttackTowerData GetAdaptedAttackTowerData(int index)
    {
        TowerData towerdata = towerDataMap.TryGetValue(index, out var data) ? data : null;
        AdaptedAttackTowerData adaptedAttackTowerData = 
            new AdaptedAttackTowerData(index, towerdata.AttackPower, towerdata.AttackSpeed, towerdata.AttackRange,towerdata.EffectTargetCount,towerdata.EffectValue,towerdata.EffectDuration);
        return adaptedAttackTowerData;
    }
    public AdaptedTrapObjectData GetAdaptedTrapObjectData(int index)
    {
        TowerData towerdata = towerDataMap.TryGetValue(index, out var data) ? data : null;
        AdaptedTrapObjectData adaptedTrapObjectData = new AdaptedTrapObjectData(index, towerdata.EffectValue, towerdata.AttackRange);
        return adaptedTrapObjectData;
    }


    public Sprite GetSprite(int towerindex)
    {
        int adjustedIndex = Utils.GetSpriteIndex(towerindex);
        adjustedIndex = adjustedIndex - 1;
        if (adjustedIndex >= 0 && adjustedIndex < TowerIcons.Length)
            return TowerIcons[adjustedIndex];
        else
            return null;
    }

    ///////////=========================������ȯ=================================/////////////////////

    public bool CanStartInteraction()
    {
        return CurrentState == InteractionState.None;
    }

    public void StartInteraction(InteractionState newState)
    {
        CurrentState = newState;
    }

    public void EndInteraction(InteractionState endState)
    {
        if (CurrentState == endState)
            CurrentState = InteractionState.None;
        Time.timeScale = 1f;
    }



    ///////////============================Ÿ�� ����Ʈ ������==============================/////////////////////

    public void AddTower(BaseTower tower)
    {
        Towers.Add(tower);
    }

    public void RemoveTower(BaseTower tower)
    {
        Towers.Remove(tower);
    }

    ///////////============================��Ÿ �޼���==============================/////////////////////

    /// <summary>
    /// Ÿ�� �ı��� ���� �����ӿ� ȣ��Ǵ� �޼���
    /// ������Ʈ�� �ı� �� ȣ������ �� �ִ� ��ġ�� ��� Ÿ���Ŵ������� ����
    /// Ÿ�� ��ġ�� Ʈ��������Ʈ�������� �ٽ� ��ġ�� �� �ֵ��� ����
    /// </summary>
    /// <param name="destroyedPos"></param>
    /// <returns></returns>
    public IEnumerator NotifyTrapObjectNextFrame(Vector2 destroyedPos)
    {
        yield return null;

        Collider2D[] hits = Physics2D.OverlapPointAll(destroyedPos, LayerMaskData.trapObject);

        foreach (var hit in hits)
        {
            TrapObject trapObject = hit.GetComponent<TrapObject>();
            if (trapObject != null)
            {
                trapObject.CanPlant();
            }
        }
    }

    /// <summary>
    /// Ÿ���� ���� �����̾� ������ �����ϴ� �޼���
    /// </summary>
    public void ApplyBossSlayer()
    {
        if (Towers == null)
        {
            return;
        }
        foreach (var tower in Towers)
        {
            if (tower is AttackTower attackTower) 
            {
                attackTower.BossSlayerBuff();
            }
        }
    }

    public void ApplyEmergencyResponse()
    {
        if (Towers == null)
        {
            return;
        }
        foreach (var tower in Towers)
        {
            tower.ApplyEmergencyResponse();
        }
    }

    public void RemoveEmergencyResponse()
    {
        if (Towers == null)
        {
            return;
        }
        foreach (var tower in Towers)
        {
            tower.RemoveEmergencyResponse();
        }
    }
}