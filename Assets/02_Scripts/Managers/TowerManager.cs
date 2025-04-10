using Unity;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TowerManager : Singleton<TowerManager>
{
    public DeckHandler hand;
    public TowerCombinationData towerCombinationData;
    public Towerbuilder towerbuilder;
}