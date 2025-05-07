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

    [Header("타워설치")]
    public SpriteRenderer attackRangeCircle;
    private Dictionary<Vector2, bool> constructCache;

    private float lastCheckTime = 0f;
    private float checkCooldown = 0.01f;
    private Vector2 lastCheckedTile = new Vector2Int(int.MinValue, int.MinValue);
    private void Start()
    {
        towerCombinationData = TowerManager.Instance.towerCombinationData;
        constructCache = new Dictionary<Vector2, bool>();
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
        isCardMoving = !isCardMoving;
    }

    public void ChangeTowerMove(BaseTower _cilkedTower)
    {
        clikedTower = _cilkedTower;
        isTowerMoving = true;
    }
    public void ChangeTowerDontMove(BaseTower _cilkedTower)
    {
        clikedTower = _cilkedTower;
        isTowerMoving = false;
    }

    ///////////=====================합성검사=====================================/////////////////////
    /// <summary>
    /// 타워를 타워위에서 클릭 해제하였을때 합성검사
    /// </summary>
    /// <param name="curPos"></param>
    public void TowerToTowerCombine(Vector2 curPos)
    {
        Vector2 CombinePos = PostionArray(curPos);
        if (cheakedTower == null) return;
        int combineTowerIndex = towerCombinationData.TryCombine(clikedTower.towerData.TowerIndex, cheakedTower.towerData.TowerIndex);

        TowerConstruct(CombinePos, combineTowerIndex);
        Destroy(cheakedTower.gameObject);
        cheakedTower = null;
        Destroy(clikedTower.gameObject);
        clikedTower = null;
        ClearConstructCache();
    }

    /// <summary>
    /// 카드를 타워 위에서 클릭 해제하였을때 합성검사
    /// </summary>
    /// <param name="curPos"></param>
    public void CardToTowerCombine(Vector2 curPos)
    {
        Vector2 CombinePos = PostionArray(curPos);
        if (cheakedTower == null) return;
        int combineTowerIndex = towerCombinationData.TryCombine(TowerManager.Instance.hand.HighlightedIndex, cheakedTower.towerData.TowerIndex);
        TowerConstruct(CombinePos, combineTowerIndex);
        Destroy(cheakedTower.gameObject);
        cheakedTower = null;
        ClearConstructCache();
    }

    /// <summary>
    /// 타워를 설치할 수 있는지 검사하는 코루틴
    /// 이미 통과한 타일은 캐시를 사용하여 경로를 검사하지 않음 
    /// </summary>
    /// <param name="curPos"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
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
            bool pathValid = NavMesh.CalculatePath(spawn.position, targetPosition.position, NavMesh.AllAreas, path);
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
    /// <param name="curPos"></param>
    /// <param name="towerIndex"></param>
    public void TowerConstruct(Vector2 curPos, int towerIndex)
    {
        Vector2 constructPos = PostionArray(curPos);

        GameObject go = Instantiate(towerPrefab, constructPos, Quaternion.identity,TowerManager.Instance.transform);
        
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

    /// <summary>
    /// 타워가 설치되었을때 타일 캐쉬 초기화
    /// </summary>
    public void ClearConstructCache()
    {
        constructCache.Clear();
    }

    /// <summary>
    /// 타일에 설치된 오브젝트가 있는지 검사하는 메서드
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public bool IsAnyObjectOnTile(Vector2 tilePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, buildBlockMask);
        return hit != null;
    }

    ///////////=====================실제 합성하는 메서드들=====================================/////////////////////
    public bool CanCardToTowerCombine(Vector2 tilePos,int cardIndex)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, LayerMaskData.tower);
        int towerIndex;
        if (hit == null) return false;
        else towerIndex = hit.GetComponent<BaseTower>().towerData.TowerIndex;
        return hit != null? towerCombinationData.TryCombine(towerIndex, cardIndex)!=-1:false;
    }

    public bool CanTowerToTowerCombine(Vector2 tilePos)
    {
        Collider2D hit = Physics2D.OverlapPoint(tilePos, LayerMaskData.tower);
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
        return towerCombinationData.TryCombine(towerIndex, clikedTower.towerData.TowerIndex) != -1;
    }


    /// <summary>
    /// 타일 스냅
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 PostionArray(Vector2 pos)
    {
        return new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }

    /// <summary>
    /// 타워가 이동할때 실제타워대신 이동하는 고스트타워의 스프라이트 결정
    /// </summary>
    /// <param name="index"></param>
    private void GetSprite(int index)
    {
        ghostTower.GetComponent<SpriteRenderer>().sprite = TowerManager.Instance.GetSprite(index);
    }

    /// <summary>
    /// 하이라이트된 카드가 타워고스트로 변경되어 움직이는 메서드
    /// </summary>
    private void HandCardMoving()
    {
        if (ghostTower == null)
        {
            ghostTower = Instantiate(ghostTowerPrefab);
            ghostTower.transform.position = PostionArray(InputManager.Instance.GetTouchWorldPosition());
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
                            if (ghostTower == null) return;
                            ghostTower.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.3f);
                            OnAttackRangeCircle(currentTile, TowerManager.Instance.GetTowerData(TowerManager.Instance.hand.HighlightedIndex));
                        }
                        else if (CanCardToTowerCombine(currentTile, TowerManager.Instance.hand.HighlightedIndex))
                        {
                            Collider2D hit = Physics2D.OverlapPoint(currentTile, LayerMaskData.tower);
                            cheakedTower = hit.GetComponent<BaseTower>();
                            Color precolor = cheakedTower.GetComponent<SpriteRenderer>().color;
                            precolor.a = 0.0f;
                            cheakedTower.GetComponent<SpriteRenderer>().color = precolor;
                            GetSprite(towerCombinationData.TryCombine(TowerManager.Instance.hand.HighlightedIndex, cheakedTower.towerData.TowerIndex));
                            ghostTower.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.3f);
                            OnAttackRangeCircle(currentTile, TowerManager.Instance.GetTowerData(towerCombinationData.TryCombine(TowerManager.Instance.hand.HighlightedIndex, cheakedTower.towerData.TowerIndex)));
                        }
                        else
                        {
                            ghostTower.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.3f);
                            EndAttackRangeCircle();
                        }
                    }));
            }
        }
    }

    /// <summary>
    /// 타워를 클릭하여 타워고스트로 변경되어 움직이는 메서드
    /// </summary>
    private void HandleTowerMovig()
    {
        if (clikedTower != null && ghostTower == null)
        {
            ghostTower = Instantiate(ghostTowerPrefab);
            ghostTower.GetComponent<SpriteRenderer>().sprite=TowerManager.Instance.GetSprite(clikedTower.towerData.TowerIndex);
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
                }
                GetSprite(clikedTower.towerData.TowerIndex);
                lastCheckedTile = currentTile;
            }
            ghostTower.transform.position = InputManager.Instance.GetTouchWorldPosition();
            if (CanTowerToTowerCombine(currentTile))
            {
                Collider2D hit = Physics2D.OverlapPoint(currentTile, LayerMaskData.tower);
                cheakedTower = hit.GetComponent<BaseTower>();
                Color precolor = cheakedTower.GetComponent<SpriteRenderer>().color;
                precolor.a = 0.0f;
                cheakedTower.GetComponent<SpriteRenderer>().color = precolor;
                ghostTower.transform.position = currentTile;
                GetSprite(towerCombinationData.TryCombine(clikedTower.towerData.TowerIndex, cheakedTower.towerData.TowerIndex));
                ghostTower.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f, 0.3f);
                OnAttackRangeCircle(currentTile, TowerManager.Instance.GetTowerData(towerCombinationData.TryCombine(clikedTower.towerData.TowerIndex, cheakedTower.towerData.TowerIndex)));
            }
            else
            {
                ghostTower.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.3f);
                EndAttackRangeCircle();
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

    /// <summary>
    /// 타워데이터에서 사거리를 받아와 설치지점에 예상 사거리를 출력
    /// </summary>
    /// <param name="constructPos"></param>
    /// <param name="towerData"></param>
    public void OnAttackRangeCircle(Vector2 constructPos,TowerData towerData)
    {
        float scaleMultiplier = 1f;

        Collider2D[] hits = Physics2D.OverlapPointAll(constructPos, LayerMaskData.platform);
        bool isOnPlatform = hits.Length > 0;

        if (isOnPlatform)
        {
            if (EnviromentManager.Instance.Season == Season.winter)
                scaleMultiplier = 1.1f;
            else
                scaleMultiplier = 1.5f;
        }

        attackRangeCircle.transform.position = constructPos;
        attackRangeCircle.transform.localScale = new Vector3(towerData.AttackRange * scaleMultiplier, towerData.AttackRange * scaleMultiplier, 1);
        attackRangeCircle.gameObject.SetActive(true);
    }

    public void EndAttackRangeCircle()
    {
        attackRangeCircle.gameObject.SetActive(false);
    }
}
