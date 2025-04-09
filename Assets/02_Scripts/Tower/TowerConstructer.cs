using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TowerConstructer : MonoBehaviour
{   
    private TowerCombinationData towerCombinationData;
    public GameObject towerPrefab;

    public Transform[] spawnPoint;
    public Transform targetPosition;
    private void Start()
    {
        towerCombinationData = TowerManager.Instance.towerCombinationData;
    }
    public bool CanCombine(int ing1, int ing2)
    {
        int result = towerCombinationData.TryCombine(ing1, ing2);
        if (result == -1) return false;
        return true;
    }

    public void TowerToTowerCombine()
    {

    }
    public void CardToTowerCombine()
    {

    }
    public IEnumerator CanConstructCoroutine(Vector2 curPos, Action<bool> callback)
    {
        Vector2 constructPos = PostionArray(curPos);
        if (IsAnyObjectOnTile(constructPos))
        {
            callback?.Invoke(false);
            yield break;
        }

        GameObject dummyTower = Instantiate(towerPrefab, constructPos, Quaternion.identity);
        yield return null;
        bool allPathsExist = true;
        NavMeshPath path = new NavMeshPath();

        foreach (Transform spawn in spawnPoint)
        {
            bool pathValid = NavMesh.CalculatePath(spawn.position, targetPosition.position, NavMesh.AllAreas, path);
            if (!pathValid || path.status != NavMeshPathStatus.PathComplete)
            {
                allPathsExist = false;
                break;
            }
        }
        Destroy(dummyTower);
        callback?.Invoke(allPathsExist);
    }

    public void TowerConstruct(Vector2 curPos,int TowerIndex)
    {
        Vector2 constructPos = PostionArray(curPos);
        GameObject go = Instantiate(towerPrefab, constructPos, Quaternion.identity);
        Tower tower = go.GetComponent<Tower>();
        tower.Init(TowerIndex);
    }

    public void TowerCombine(Tower ing1, Tower ing2)
    {
        int result = towerCombinationData.TryCombine(ing1.towerdata.TowerIndex, ing2.towerdata.TowerIndex);
        if (result == -1) return;
        Vector2 combinePos = InputManager.Instance.GetTouchWorldPosition();
        Vector2 constructPos = PostionArray(combinePos);
        Destroy(ing1.gameObject);
        Destroy(ing2.gameObject);
    }

    public bool IsAnyObjectOnTile(Vector2 tilePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos);
        return hit != null;
    }
    public Vector2 PostionArray(Vector2 pos)
    {
        return new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }
}
