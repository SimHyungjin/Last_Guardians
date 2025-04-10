using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Towerbuilder : MonoBehaviour
{
    [Header("타워 조합")]
    [SerializeField] private LayerMask buildBlockMask;
    public Tower CheakedTower = null;

    [Header("마우스이동")]
    private TowerCombinationData towerCombinationData;
    public GameObject towerPrefab;
    public GameObject ghostTowerPrefab;
    private GameObject ghostTower;
    public bool isCardMoving;

    [Header("이동경로파악")]
    public Transform[] spawnPoint;
    public Transform targetPosition;



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
                ghostTower.transform.position = PostionArray(InputManager.Instance.GetTouchWorldPosition());
                SpriteRenderer ghostsprite = ghostTower.GetComponent<SpriteRenderer>();
                GetSprite(TowerManager.Instance.hand.HighlightedIndex);
            }
            else
            {
                if (Time.time - lastCheckTime < checkCooldown) return;
                Vector2 currentTile = PostionArray(InputManager.Instance.GetTouchWorldPosition());
                if (currentTile != lastCheckedTile)
                {
                    if (CheakedTower != null)
                    {
                        Color precolor = CheakedTower.GetComponent<SpriteRenderer>().color;
                        precolor.a = 1.0f;
                        CheakedTower.GetComponent<SpriteRenderer>().color = precolor;
                        CheakedTower = null;
                        GetSprite(TowerManager.Instance.hand.HighlightedIndex);
                    }

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
                                Collider2D hit = Physics2D.OverlapPoint(currentTile, LayerMask.GetMask("Tower"));
                                CheakedTower = hit.GetComponent<Tower>();
                                Color precolor = CheakedTower.GetComponent<SpriteRenderer>().color;
                                precolor.a = 0.0f;
                                CheakedTower.GetComponent<SpriteRenderer>().color = precolor;
                                GetSprite(towerCombinationData.TryCombine(TowerManager.Instance.hand.HighlightedIndex, CheakedTower.towerdata.TowerIndex));
                                ghostTower.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.3f);
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
    public void TowerToTowerCombine()
    {

    }
    public void CardToTowerCombine(Vector2 curPos)
    {
        Vector2 CombinePos = PostionArray(curPos);
        int combineTowerIndex = towerCombinationData.TryCombine(TowerManager.Instance.hand.HighlightedIndex, CheakedTower.towerdata.TowerIndex);
        Tower combineTower = Instantiate(towerPrefab, CombinePos, Quaternion.identity).GetComponent<Tower>();
        combineTower.Init(combineTowerIndex);
        Destroy(TowerManager.Instance.towerbuilder.CheakedTower.gameObject);
        TowerManager.Instance.towerbuilder.CheakedTower = null;
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

        GameObject dummyTower = Instantiate(towerPrefab, constructPos, Quaternion.identity);
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
        GameObject go = Instantiate(towerPrefab, constructPos, Quaternion.identity);
        Tower tower = go.GetComponent<Tower>();
        tower.Init(TowerIndex);
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
        return hit != null? towerCombinationData.TryCombine(towerIndex, cardIndex)!=-1:false;
    }
    public Vector2 PostionArray(Vector2 pos)
    {
        return new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }

    private void GetSprite(int index)
    {
        string path = $"Assets/90_SO/Tower/TestTower{index}.asset";
        TowerData towerdata = AssetDatabase.LoadAssetAtPath<TowerData>(path);
        ghostTower.GetComponent<SpriteRenderer>().sprite = towerdata.towerSprite;
    }
}
