using Unity;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TowerManager : Singleton<TowerManager>
{
    public HandCardLayout hand;
    public TowerCombinationData towerCombinationData;
    public TowerConstructer towerConstructer;

    public void Start()
    {
    }
}