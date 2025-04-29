using Unity;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;
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
    }

    public IEnumerator NotifyTrapObjectNextFrame(Vector2 destroyedPos)
    {
        yield return null; 
        
        Collider2D[] hits = Physics2D.OverlapPointAll(destroyedPos,LayerMaskData.trapObject);

        foreach (var hit in hits)
        {
            TrapObject trapObject = hit.GetComponent<TrapObject>();
            if (trapObject != null)
            {
                trapObject.CanPlant();
                Debug.Log(trapObject.transform);
            }
        }
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

    public void AddTower(BaseTower tower)
    {
        Towers.Add(tower);
    }

    public void RemoveTower(BaseTower tower)
    {
        Towers.Remove(tower);
    }
}