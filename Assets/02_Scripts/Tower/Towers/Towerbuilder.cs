using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;   

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

    [Header("타워설치")]
    public SpriteRenderer attackRangeCircle;
    private Dictionary<Vector2, bool> constructCache;

    private float lastCheckTime = 0f;
    private float checkCooldown = 0.01f;
    private Vector2 lastCheckedTile = new Vector2Int(int.MinValue, int.MinValue);

    // ← 추가: InGameManager에서 캐싱된 Tilemap 참조
    private Tilemap obstacleTilemap;

    private void Start()
    {
        towerCombinationData = TowerManager.Instance.towerCombinationData;
        constructCache = new Dictionary<Vector2, bool>();

        // ← 추가: 타일맵 참조 가져오기
        obstacleTilemap = InGameManager.Instance.ObstacleTilemap;
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

    ///////////=====================상태변환=====================================/////////////////////
    public void ChangeCardMove()
    {
        Time.timeScale = 0.2f;
        isCardMoving = true;
    }

    public void ChangeCardDontMove()
    {
        Time.timeScale = 1f;
        lastCheckedTile = new Vector2Int(int.MinValue, int.MinValue);
        EndAttackRangeCircle();
        isCardMoving = false;
    }

    public void ChangeTowerMove(BaseTower _cilkedTower)
    {
        Time.timeScale = 0.2f;
        clikedTower = _cilkedTower;
        isTowerMoving = true;
    }

    public void ChangeTowerDontMove(BaseTower _cilkedTower)
    {
        clikedTower = _cilkedTower;
        isTowerMoving = false;
    }

    ///////////=====================합성검사=====================================/////////////////////
    public void TowerToTowerCombine(Vector2 curPos)
    {
        Vector2 CombinePos = PostionArray(curPos);
        if (cheakedTower == null) return;
        int combineTowerIndex = towerCombinationData.TryCombine(
            clikedTower.towerData.TowerIndex,
            cheakedTower.towerData.TowerIndex
        );

        TowerConstruct(CombinePos, combineTowerIndex);
        Destroy(cheakedTower.gameObject);
        cheakedTower = null;
        Destroy(clikedTower.gameObject);
        clikedTower = null;
        ClearConstructCache();
    }

    public void CardToTowerCombine(Vector2 curPos)
    {
        Vector2 CombinePos = PostionArray(curPos);
        if (cheakedTower == null) return;
        int combineTowerIndex = towerCombinationData.TryCombine(
            TowerManager.Instance.hand.HighlightedIndex,
            cheakedTower.towerData.TowerIndex
        );
        TowerConstruct(CombinePos, combineTowerIndex);
        Destroy(cheakedTower.gameObject);
        cheakedTower = null;
        ClearConstructCache();
    }

    public IEnumerator CanConstructCoroutine(Vector2 curPos, Action<bool> callback)
    {
        Vector2 constructPos = PostionArray(curPos);

        if (constructCache.TryGetValue(constructPos, out bool cachedResult))
        {
            Debug.Log($"[캐시] 타일 검사 결과: {cachedResult}");
            callback?.Invoke(cachedResult);
            yield break;
        }
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
            bool pathValid = NavMesh.CalculatePath(
                spawn.position,
                targetPosition.position,
                NavMesh.AllAreas,
                path
            );
            if (!pathValid || path.status != NavMeshPathStatus.PathComplete)
            {
                Debug.Log("경로가 없음");
                allPathsExist = false;
                break;
            }
        }
        Destroy(dummyTower);
        constructCache[constructPos] = allPathsExist;
        callback?.Invoke(allPathsExist);
    }

    /// <summary>
    /// 타워를 설치하는 메서드
    /// </summary>
    public void TowerConstruct(Vector2 curPos, int towerIndex)
    {
        Vector2 constructPos = PostionArray(curPos);

        // ← 추가: 해당 셀의 룰타일 제거
        if (obstacleTilemap != null)
        {
            Vector3Int cell = obstacleTilemap.WorldToCell(constructPos);
            obstacleTilemap.SetTile(cell, null);
            obstacleTilemap.RefreshTile(cell);
        }
        SoundManager.Instance.PlaySFX("TowerBuild");
        // 기존 타워 생성 로직
        GameObject go = Instantiate(
            towerPrefab,
            constructPos,
            Quaternion.identity,
            TowerManager.Instance.transform
        );

        TowerData data = TowerManager.Instance.GetTowerData(towerIndex);
        if (data == null)
        {
            Debug.LogError($"[TowerConstruct] TowerData를 불러올 수 없습니다: {towerIndex}");
            return;
        }

        if (data.ProjectileType == ProjectileType.Buff)
        {
            BuffTower tower = go.AddComponent<BuffTower>();
            tower.Init(data);
        }
        else if (data.ProjectileType == ProjectileType.TrapObject)
        {
            TrapObjectTower tower = go.AddComponent<TrapObjectTower>();
            tower.Init(data);
        }
        else
        {
            AttackTower tower = go.AddComponent<AttackTower>();
            tower.Init(data);
        }
        ClearConstructCache();
    }

    public void ClearConstructCache()
    {
        constructCache.Clear();
    }

    public bool IsAnyObjectOnTile(Vector2 tilePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, buildBlockMask);
        return hit != null;
    }

    public bool CanCardToTowerCombine(Vector2 tilePos, int cardIndex)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, LayerMaskData.tower);
        if (hit == null) return false;
        int towerIndex = hit.GetComponent<BaseTower>().towerData.TowerIndex;
        return towerCombinationData.TryCombine(towerIndex, cardIndex) != -1;
    }

    public bool CanTowerToTowerCombine(Vector2 tilePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, LayerMaskData.tower);
        if (hit == null) return false;
        BaseTower tower = hit.GetComponent<BaseTower>();
        if (tower == null || tower == clikedTower) return false;
        int towerIndex = tower.towerData.TowerIndex;
        return towerCombinationData.TryCombine(
            towerIndex,
            clikedTower.towerData.TowerIndex
        ) != -1;
    }

    public Vector2 PostionArray(Vector2 pos)
    {
        return new Vector2(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y)
        );
    }

    private void GetSprite(int index)
    {
        ghostTower.GetComponent<SpriteRenderer>().sprite =
            TowerManager.Instance.GetSprite(index);
    }

    private void HandCardMoving()
    {
        if (ghostTower == null)
        {
            ghostTower = Instantiate(ghostTowerPrefab);
            ghostTower.transform.position = PostionArray(
                InputManager.Instance.GetTouchWorldPosition()
            );
            GetSprite(TowerManager.Instance.hand.HighlightedIndex);
        }
        else
        {
            if (Time.time - lastCheckTime < checkCooldown) return;
            Vector2 currentTile = PostionArray(
                InputManager.Instance.GetTouchWorldPosition()
            );
            if (currentTile != lastCheckedTile)
            {
                if (cheakedTower != null)
                {
                    Color c = cheakedTower.GetComponent<SpriteRenderer>().color;
                    c.a = 1f;
                    cheakedTower.GetComponent<SpriteRenderer>().color = c;
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
                            ghostTower.GetComponent<SpriteRenderer>().color =
                                new Color(0f, 1f, 0f, 0.3f);
                            OnAttackRangeCircle(
                                currentTile,
                                TowerManager.Instance.GetTowerData(
                                    TowerManager.Instance.hand.HighlightedIndex
                                )
                            );
                        }
                        else if (CanCardToTowerCombine(
                            currentTile,
                            TowerManager.Instance.hand.HighlightedIndex
                        ))
                        {
                            Collider2D hit = Physics2D.OverlapPoint(
                                currentTile,
                                LayerMaskData.tower
                            );
                            cheakedTower = hit.GetComponent<BaseTower>();
                            Color c = cheakedTower.GetComponent<SpriteRenderer>().color;
                            c.a = 0f;
                            cheakedTower.GetComponent<SpriteRenderer>().color = c;
                            GetSprite(
                                towerCombinationData.TryCombine(
                                    TowerManager.Instance.hand.HighlightedIndex,
                                    cheakedTower.towerData.TowerIndex
                                )
                            );
                            ghostTower.GetComponent<SpriteRenderer>().color =
                                new Color(0f, 1f, 0f, 0.3f);
                            OnAttackRangeCircle(
                                currentTile,
                                TowerManager.Instance.GetTowerData(
                                    towerCombinationData.TryCombine(
                                        TowerManager.Instance.hand.HighlightedIndex,
                                        cheakedTower.towerData.TowerIndex
                                    )
                                )
                            );
                        }
                        else
                        {
                            ghostTower.GetComponent<SpriteRenderer>().color =
                                new Color(1f, 0f, 0f, 0.3f);
                            EndAttackRangeCircle();
                        }
                    }
                ));
            }
        }
    }

    private void HandleTowerMovig()
    {
        if (clikedTower != null && ghostTower == null)
        {
            ghostTower = Instantiate(ghostTowerPrefab);
            ghostTower.GetComponent<SpriteRenderer>().sprite =
                TowerManager.Instance.GetSprite(clikedTower.towerData.TowerIndex);
            Color c = clikedTower.sprite.color;
            c.a = 0.3f;
            clikedTower.sprite.color = c;
        }
        else
        {
            Vector2 currentTile = PostionArray(
                InputManager.Instance.GetTouchWorldPosition()
            );
            if (currentTile != lastCheckedTile)
            {
                if (cheakedTower != null)
                {
                    Color c = cheakedTower.GetComponent<SpriteRenderer>().color;
                    c.a = 1f;
                    cheakedTower.GetComponent<SpriteRenderer>().color = c;
                    cheakedTower = null;
                }
                GetSprite(clikedTower.towerData.TowerIndex);
                lastCheckedTile = currentTile;
            }
            ghostTower.transform.position =
                InputManager.Instance.GetTouchWorldPosition();
            if (CanTowerToTowerCombine(currentTile))
            {
                Collider2D hit = Physics2D.OverlapPoint(
                    currentTile,
                    LayerMaskData.tower
                );
                cheakedTower = hit.GetComponent<BaseTower>();
                Color c = cheakedTower.GetComponent<SpriteRenderer>().color;
                c.a = 0f;
                cheakedTower.GetComponent<SpriteRenderer>().color = c;
                ghostTower.transform.position = currentTile;
                GetSprite(
                    towerCombinationData.TryCombine(
                        clikedTower.towerData.TowerIndex,
                        cheakedTower.towerData.TowerIndex
                    )
                );
                ghostTower.GetComponent<SpriteRenderer>().color =
                    new Color(0f, 1f, 0f, 0.3f);
                OnAttackRangeCircle(
                    currentTile,
                    TowerManager.Instance.GetTowerData(
                        towerCombinationData.TryCombine(
                            clikedTower.towerData.TowerIndex,
                            cheakedTower.towerData.TowerIndex
                        )
                    )
                );
            }
            else
            {
                ghostTower.GetComponent<SpriteRenderer>().color =
                    new Color(1f, 0f, 0f, 0.3f);
                EndAttackRangeCircle();
            }
        }
    }

    private void ResetMoving()
    {
        if (clikedTower != null)
        {
            Color c = clikedTower.sprite.color;
            c.a = 1f;
            clikedTower.sprite.color = c;
            clikedTower = null;
        }
        if (ghostTower != null)
        {
            Destroy(ghostTower);
            ghostTower = null;
        }
    }

    public void OnAttackRangeCircle(Vector2 constructPos, TowerData towerData)
    {
        float scaleMultiplier = 1f;
        int AttackRangeupgradeLevel = TowerManager.Instance.towerUpgradeData.currentLevel[(int)TowerUpgradeType.AttackRange];
        float Upgradescale= TowerManager.Instance.towerUpgradeValueData.towerUpgradeValues[(int)TowerUpgradeType.AttackRange].levels[AttackRangeupgradeLevel];
        Collider2D[] hits = Physics2D.OverlapPointAll(
            constructPos,
            LayerMaskData.platform
        );
        bool isOnPlatform = hits.Length > 0;

        if (isOnPlatform)
            scaleMultiplier =
                EnviromentManager.Instance.Season == Season.winter
                    ? 1.1f
                    : 1.5f;

        attackRangeCircle.transform.position = constructPos;
        attackRangeCircle.transform.localScale =
            new Vector3(
                towerData.AttackRange * scaleMultiplier* Upgradescale,
                towerData.AttackRange * scaleMultiplier* Upgradescale,
                1
            );
        attackRangeCircle.gameObject.SetActive(true);
    }

    public void EndAttackRangeCircle()
    {
        attackRangeCircle.gameObject.SetActive(false);
    }
}
