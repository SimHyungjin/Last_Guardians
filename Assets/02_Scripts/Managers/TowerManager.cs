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
    public DeckHandler hand;
    public TowerCombinationData towerCombinationData;
    public Towerbuilder towerbuilder;
    public ProjectileFactory projectileFactory;
    public GameObject TrapObjectPrefab;
    private Dictionary<int, TowerData> towerDataMap;
    public InteractionState CurrentState { get; private set; } = InteractionState.None;

    public void Start()
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
        Debug.Log("타워해제하니까 트랩 재검사하거라");
        yield return null; 
        
        Collider2D[] hits = Physics2D.OverlapPointAll(destroyedPos, LayerMask.GetMask("TrapObject"));

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
}