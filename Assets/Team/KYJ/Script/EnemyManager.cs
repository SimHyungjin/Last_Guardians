using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private GameObject[] normalPrefabs;
    [SerializeField] private GameObject[] bountyPrefabs;
    [SerializeField] private GameObject[] bossPrefabs;

    private BaseMonster monster;

    private void SpawnEnemy()
    {

    }
}
