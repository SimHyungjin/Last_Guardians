using Unity;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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

    public InteractionState CurrentState { get; private set; } = InteractionState.None;
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
}