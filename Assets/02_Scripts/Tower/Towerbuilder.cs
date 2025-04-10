using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Towerbuilder : MonoBehaviour
{
    [SerializeField] private LayerMask buildBlockMask;
    private TowerCombinationData towerCombinationData;
    public GameObject dummytowerPrefab;

    public GameObject ghostTowerPrefab;
    private GameObject ghostTower;

    public Transform[] spawnPoint;
    public Transform targetPosition;

    public bool isCardMoving;

    private float lastCheckTime = 0f;
    private float checkCooldown = 0.2f;
    private Vector2 lastCheckedTile = new Vector2Int(int.MinValue, int.MinValue);
    private void Start()
    {
        towerCombinationData = TowerManager.Instance.towerCombinationData;
    }

    private void Update()
    {
        if (isCardMoving)
        {
            if (ghostTower == null)
            {
                ghostTower = Instantiate(ghostTowerPrefab);
                SpriteRenderer ghostsprite = ghostTower.GetComponent<SpriteRenderer>();
                string path = $"Assets/90_SO/Tower/TestTower{TowerManager.Instance.hand.HighlightedIndex}.asset";
                TowerData towerdata = AssetDatabase.LoadAssetAtPath<TowerData>(path);
                ghostsprite.sprite = towerdata.towerSprite;
            }
            else
            {
                if (Time.time - lastCheckTime < checkCooldown) return;
                Vector2 currentTile = PostionArray(InputManager.Instance.GetTouchWorldPosition());
                if (currentTile != lastCheckedTile)
                {
                    lastCheckTime = Time.time;
                    lastCheckedTile = currentTile;
                    ghostTower.transform.position = currentTile;
                    StartCoroutine(CanConstructCoroutine(
                        currentTile,
                        (canPlace) =>
                        {
                            if (canPlace)
                            {
                                ghostTower.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.3f);
                            }
                            else if(CanCardToTowerCombine(currentTile, TowerManager.Instance.hand.HighlightedIndex))
                            {
                                Debug.Log("타워 조합 가능");
                            }
                            else
                            {
                                ghostTower.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.3f);
                            }
                        }));
                }
            }
        }
        else
        {
            if (ghostTower != null)
            {
                Destroy(ghostTower);
                ghostTower = null;
            }
        }
    }
    public void ChangeCardMove()
    {
        Debug.Log("ChangeCardMove");
        isCardMoving = !isCardMoving;
    }
    public int CanCombine(int ing1, int ing2)
    {
        int result = towerCombinationData.TryCombine(ing1, ing2);
        Debug.Log($"result : {result}");
        //if (result == -1) return false;
        //return true;
        return result;
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
            Debug.Log("타일에 충돌체있음");
            callback?.Invoke(false);
            yield break;
        }

        GameObject dummyTower = Instantiate(dummytowerPrefab, constructPos, Quaternion.identity);
        yield return null;
        bool allPathsExist = true;
        NavMeshPath path = new NavMeshPath();

        foreach (Transform spawn in spawnPoint)
        {
            bool pathValid = NavMesh.CalculatePath(spawn.position, targetPosition.position, NavMesh.AllAreas, path);
            if (!pathValid || path.status != NavMeshPathStatus.PathComplete)
            {
                Debug.Log("경로가 없음");
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
        GameObject go = Instantiate(dummytowerPrefab, constructPos, Quaternion.identity);
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
        Collider2D hit = Physics2D.OverlapPoint(tilePos, buildBlockMask);
        Debug.Log(hit);
        return hit != null;
    }
    public bool CanCardToTowerCombine(Vector2 tilePos,int cardIndex)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, LayerMask.GetMask("Tower"));
        int towerIndex;
        if (hit == null) return false;
        else towerIndex = hit.GetComponent<Tower>().towerdata.TowerIndex;
        return hit != null?CanCombine(towerIndex, cardIndex)!=-1:false;
        //return hit != null;
    }
    public Vector2 PostionArray(Vector2 pos)
    {
        return new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }
}
