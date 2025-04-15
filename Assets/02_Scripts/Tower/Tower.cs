using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tower : MonoBehaviour
{
    [Header("타워 데이터")]
    public TowerData towerData;


    [Header("타워 결합")]
    public GameObject towerGhostPrefab;
    public bool isMoving;
    public bool isCliked;
    public SpriteRenderer sprite;

    [Header("공격")]
    private Transform target;
    private float lastCheckTime = 0f;
    [SerializeField] private LayerMask monsterLayer;
    public ProjectileFactory projectileFactory;

    private GameObject towerGhost;
    Vector2 curPos;

    public void Init(int index)
    {
        projectileFactory = FindObjectOfType<ProjectileFactory>();
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        towerData = Resources.Load<TowerData>($"SO/Tower/Tower{index}");
        sprite = GetComponent<SpriteRenderer>();
        if (towerData != null)
        {
            int spriteIndex = (towerData.TowerIndex > 49) ? towerData.TowerIndex - 49 : towerData.TowerIndex;
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
                sprite.sprite = towerData.atlas.GetSprite($"Tower_{spriteIndex}");
                towerGhost = towerData.towerGhostPrefab;
            }
        }
    }
    private void Update()
    {

        if (isMoving)
        {
            curPos = InputManager.Instance.GetTouchWorldPosition();
            towerGhost.transform.position = curPos;
        }

        if (target == null || !IsInRange(target.position))
        {
            FindTarget();
            return;
        }

        if (Time.time - lastCheckTime < towerData.AttackSpeed) return;
        {
            FindTarget();
            if (projectileFactory == null || towerData == null)
            {
                Debug.LogError("ProjectileFactory or TowerData is null in Tower.Update");
                return;  // 필수 객체가 null이라면 Update에서 더 이상 진행하지 않음
            }
            lastCheckTime = Time.time;
            Attack();
        }
    }


    bool IsInRange(Vector3 targetPos)
    {
        return Vector3.Distance(transform.position, targetPos) <= towerData.AttackRange;
    }

    void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange, monsterLayer);

        float closestDist = float.MaxValue;
        Transform closest = null;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = hit.transform;
            }
        }

        target = closest;
    }

    void Attack()
    {
        if (target == null || !IsInRange(target.position))
            return;
        Debug.Log($"타워 공격 : {towerData.TowerName} / 타겟 : {target.name}");
        projectileFactory.SpawnAndLaunch(target.position,towerData,this.transform);
    }

    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
        if (!isCliked)
        {
            curPos = InputManager.Instance.GetTouchWorldPosition();
            Collider2D hit = Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Tower"));
            if (hit != null && hit.gameObject == this.gameObject && TowerManager.Instance.CanStartInteraction())
            {
                isCliked = true;
                TowerManager.Instance.towerbuilder.ChangeTowerMove(this);
                TowerManager.Instance.StartInteraction(InteractionState.TowerMoving);
            }
        }
    }

    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        if (isCliked)
        {
            isCliked = false;
            TowerManager.Instance.towerbuilder.ChangeTowerMove(this);
            TowerManager.Instance.EndInteraction(InteractionState.TowerMoving);
            if (TowerManager.Instance.towerbuilder.CanTowerToTowerCombine(InputManager.Instance.GetTouchWorldPosition()))
            {
                TowerManager.Instance.towerbuilder.TowerToTowerCombine(InputManager.Instance.GetTouchWorldPosition());
            }
        }
    }
    private void OnDestroy()
    {
        InputManager.Instance?.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
        isMoving = false;
    }
}


