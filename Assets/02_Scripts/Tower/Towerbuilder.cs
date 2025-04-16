using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.U2D;

public class Towerbuilder : MonoBehaviour
{
    [Header("타워 조합")]
    [SerializeField] private LayerMask buildBlockMask;
    public BaseTower cheakedTower = null;
    public BaseTower clikedTower = null;

    [Header("마우스이동")]
    private TowerCombinationData towerCombinationData;
    public GameObject towerPrefab;
    public GameObject ghostTowerPrefab;
    private GameObject ghostTower;
    public bool isCardMoving;
    public bool isTowerMoving;

    [Header("이동경로파악")]
    public Transform[] spawnPoint;
    public Transform targetPosition;
    public GameObject dummyTowerPrefab;



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
            HandCardMoving();
        }
        else if (isTowerMoving)
        {
            HandleTowerMovig();
        }
        else
        {
            ResetMoving();
        }
    }

    public void ChangeCardMove()
    {
        isCardMoving = !isCardMoving;
    }

    public void ChangeTowerMove(BaseTower _cilkedTower)
    {
        clikedTower = _cilkedTower;
        isTowerMoving = !isTowerMoving;
    }

    public void TowerToTowerCombine(Vector2 curPos)
    {
        Vector2 CombinePos = PostionArray(curPos);
        int combineTowerIndex = towerCombinationData.TryCombine(clikedTower.towerData.TowerIndex, cheakedTower.towerData.TowerIndex);

        TowerConstruct(CombinePos, combineTowerIndex);
        Destroy(cheakedTower.gameObject);
        cheakedTower = null;
        Destroy(clikedTower.gameObject);
        clikedTower = null;
    }

    public void CardToTowerCombine(Vector2 curPos)
    {
        Vector2 CombinePos = PostionArray(curPos);
        int combineTowerIndex = towerCombinationData.TryCombine(TowerManager.Instance.hand.HighlightedIndex, cheakedTower.towerData.TowerIndex);
        TowerConstruct(CombinePos, combineTowerIndex);
        Destroy(cheakedTower.gameObject);
        cheakedTower = null;
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

        GameObject dummyTower = Instantiate(dummyTowerPrefab, constructPos, Quaternion.identity);
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

    public void TowerConstruct(Vector2 curPos, int towerIndex)
    {
        Vector2 constructPos = PostionArray(curPos);
        GameObject go = Instantiate(towerPrefab, constructPos, Quaternion.identity);

        TowerData data = TowerManager.Instance.GetTowerData(towerIndex);

        if (data == null)
        {
            Debug.LogError($"[TowerConstruct] TowerData를 불러올 수 없습니다: {towerIndex}");
            return;
        }

        if (data.ProjectileType == ProjectileType.Buff)
        {
            BuffTower tower = go.AddComponent<BuffTower>();
            tower.Init(data); // Init 안에서 다시 SO를 불러와도 됨
        }
        else
        {
            AttackTower tower = go.AddComponent<AttackTower>();
            tower.Init(data);
        }
    }

    public bool IsAnyObjectOnTile(Vector2 tilePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, buildBlockMask);
        return hit != null;
    }

    public bool CanCardToTowerCombine(Vector2 tilePos,int cardIndex)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, LayerMask.GetMask("Tower"));
        int towerIndex;
        if (hit == null) return false;
        else towerIndex = hit.GetComponent<BaseTower>().towerData.TowerIndex;
        return hit != null? towerCombinationData.TryCombine(towerIndex, cardIndex)!=-1:false;
    }

    public bool CanTowerToTowerCombine(Vector2 tilePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, LayerMask.GetMask("Tower"));
        if (hit == null)
        {
            return false;
        }
        BaseTower tower = hit.GetComponent<BaseTower>();
        if (tower == null)
        {
            return false;
        }
        if (tower == clikedTower)
        {
            return false;
        }
        int towerIndex = tower.towerData.TowerIndex;
        Debug.Log(towerCombinationData.TryCombine(towerIndex, clikedTower.towerData.TowerIndex));
        return towerCombinationData.TryCombine(towerIndex, clikedTower.towerData.TowerIndex) != -1;
    }



    public Vector2 PostionArray(Vector2 pos)
    {
        return new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }

    private void GetSprite(int index)
    {
        TowerData towerData = TowerManager.Instance.GetTowerData(index);
        int spriteIndex;
        if (towerData.TowerIndex > 49 && towerData.TowerIndex < 99)
        {
            spriteIndex = towerData.TowerIndex - 49;
        }
        else if (towerData.TowerIndex > 98 && towerData.TowerIndex < 109)
        {
            spriteIndex = towerData.TowerIndex - 98;
        }
        else if (towerData.TowerIndex > 108)
        {
            spriteIndex = towerData.TowerIndex - 59;
        }
        else
        {
            spriteIndex = towerData.TowerIndex;
        }
        ghostTower.GetComponent<SpriteRenderer>().sprite = towerData.atlas.GetSprite($"Tower_{spriteIndex}");

    }

    private void HandCardMoving()
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
                if (cheakedTower != null)
                {
                    Color precolor = cheakedTower.GetComponent<SpriteRenderer>().color;
                    precolor.a = 1.0f;
                    cheakedTower.GetComponent<SpriteRenderer>().color = precolor;
                    cheakedTower = null;
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
                        else if (CanCardToTowerCombine(currentTile, TowerManager.Instance.hand.HighlightedIndex))
                        {
                            Collider2D hit = Physics2D.OverlapPoint(currentTile, LayerMask.GetMask("Tower"));
                            cheakedTower = hit.GetComponent<BaseTower>();
                            Color precolor = cheakedTower.GetComponent<SpriteRenderer>().color;
                            precolor.a = 0.0f;
                            cheakedTower.GetComponent<SpriteRenderer>().color = precolor;
                            GetSprite(towerCombinationData.TryCombine(TowerManager.Instance.hand.HighlightedIndex, cheakedTower.towerData.TowerIndex));
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

    private void HandleTowerMovig()
    {
        if (clikedTower != null && ghostTower == null)
        {
            ghostTower = Instantiate(ghostTowerPrefab);
            int spriteIndex;
            if (clikedTower.towerData.TowerIndex > 49 && clikedTower.towerData.TowerIndex < 99)
            {
                spriteIndex = clikedTower.towerData.TowerIndex - 49;
            }
            else if (clikedTower.towerData.TowerIndex > 98 && clikedTower.towerData.TowerIndex < 109)
            {
                spriteIndex = clikedTower.towerData.TowerIndex - 98;
            }
            else if (clikedTower.towerData.TowerIndex > 108)
            {
                spriteIndex = clikedTower.towerData.TowerIndex - 59;
            }
            else
            {
                spriteIndex = clikedTower.towerData.TowerIndex;
            }
            ghostTower.GetComponent<SpriteRenderer>().sprite = clikedTower.towerData.atlas.GetSprite($"Tower_{spriteIndex}");

            clikedTower.sprite.color = new Color(clikedTower.sprite.color.r, clikedTower.sprite.color.g, clikedTower.sprite.color.b, 0.3f);
        }
        else
        {
            Vector2 currentTile = PostionArray(InputManager.Instance.GetTouchWorldPosition());
            if (currentTile != lastCheckedTile)
            {
                if (cheakedTower != null)
                {
                    Color precolor = cheakedTower.GetComponent<SpriteRenderer>().color;
                    precolor.a = 1.0f;
                    cheakedTower.GetComponent<SpriteRenderer>().color = precolor;
                    cheakedTower = null;
                    GetSprite(clikedTower.towerData.TowerIndex);
                }
            }
            ghostTower.transform.position = InputManager.Instance.GetTouchWorldPosition();
            if (CanTowerToTowerCombine(currentTile))
            {
                Collider2D hit = Physics2D.OverlapPoint(currentTile, LayerMask.GetMask("Tower"));
                cheakedTower = hit.GetComponent<BaseTower>();
                Color precolor = cheakedTower.GetComponent<SpriteRenderer>().color;
                precolor.a = 0.0f;
                cheakedTower.GetComponent<SpriteRenderer>().color = precolor;
                ghostTower.transform.position = currentTile;
                GetSprite(towerCombinationData.TryCombine(clikedTower.towerData.TowerIndex, cheakedTower.towerData.TowerIndex));
                ghostTower.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.3f);
            }
            else
            {
                ghostTower.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.3f);
            }
        }
    }

    private void ResetMoving()
    {
        if (clikedTower != null)
        {
            clikedTower.sprite.color = new Color(clikedTower.sprite.color.r, clikedTower.sprite.color.g, clikedTower.sprite.color.b, 1f);
            clikedTower = null;
        }
        if (ghostTower != null)
        {
            Destroy(ghostTower);
            ghostTower = null;
        }
    }
}
