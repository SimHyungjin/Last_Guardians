using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Tower : MonoBehaviour
{
    [Header("타워 데이터")]
    public TowerData towerdata;


    [Header("타워 결합")]
    public GameObject towerGhostPrefab;
    public bool isMoving;
    public bool isCliked;
    public SpriteRenderer sprite;
    [Header("공격")]
    public ProjectileBase projectile;
    private Transform target;
    private float lastCheckTime = 0f;
    [SerializeField] private LayerMask monsterLayer;

    private GameObject towerGhost;
    Vector2 curPos;

    public void Init(int index)
    {
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        towerdata = Resources.Load<TowerData>($"SO/Tower/Tower{index}");
        sprite = GetComponent<SpriteRenderer>();
        if (towerdata != null)
        {
            int spriteIndex = (towerdata.TowerIndex>49)? towerdata.TowerIndex-49: towerdata.TowerIndex;
            sprite.sprite = towerdata.atlas.GetSprite($"Tower_{spriteIndex}");
            towerGhost = towerdata.towerGhostPrefab;
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

        if (Time.time - lastCheckTime < towerdata.AttackSpeed) return;
        {
            lastCheckTime = Time.time;
            Attack();
        }
    }


    bool IsInRange(Vector3 targetPos)
    {
        return Vector3.Distance(transform.position, targetPos) <= towerdata.AttackRange;
    }

    void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerdata.AttackRange, monsterLayer);

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
        Debug.Log($"타워 공격 : {towerdata.TowerName} / 타겟 : {target.name}");
        // projectile.Launch(target.position, towerdata.AttackPower, true); //요청한 방식 그대로
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f); 
        Gizmos.DrawWireSphere(transform.position, towerdata.AttackRange);
    }
    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
        if (!isCliked)
        {
            curPos = InputManager.Instance.GetTouchWorldPosition();
            Collider2D hit = Physics2D.OverlapPoint(curPos, LayerMask.GetMask("Tower"));
            if (hit != null && hit.gameObject == this.gameObject)
            {
                isCliked = true;
                TowerManager.Instance.towerbuilder.ChangeTowerMove(this);
            }
        }
    }

    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        if (isCliked)
        {
            isCliked = false;
            TowerManager.Instance.towerbuilder.ChangeTowerMove(this);
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

