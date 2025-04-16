using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public abstract class BaseTower : MonoBehaviour
{
    [Header("타워 데이터")]
    public TowerData towerData;


    [Header("타워 결합")]
    public GameObject towerGhostPrefab;
    public bool isMoving;
    public bool isCliked;
    public SpriteRenderer sprite;

    //[Header("공격")]
    //[SerializeField]private Transform target;
    //private float lastCheckTime = 0f;
    //[SerializeField] private LayerMask monsterLayer;
    //public ProjectileFactory projectileFactory;
    ////버프목록 -> 팩토리에 전달
    //private BaseMonster currentTargetMonster;

    private GameObject towerGhost;
    Vector2 curPos;

    public virtual void Init(TowerData _towerData)
    {
        towerData = _towerData;
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        towerGhostPrefab = _towerData.towerGhostPrefab;
        sprite = GetComponent<SpriteRenderer>();
        if (towerData != null)
        {
            int spriteIndex;
            if (towerData.TowerIndex > 49 && towerData.TowerIndex < 99)
            {
                spriteIndex = towerData.TowerIndex - 49;
            }
            else if (towerData.TowerIndex >= 99 && towerData.TowerIndex < 109)
            {
                spriteIndex = towerData.TowerIndex - 98;
            }
            else if (towerData.TowerIndex >= 109)
            {
                spriteIndex = towerData.TowerIndex - 59;
            }
            else
            {
                spriteIndex = towerData.TowerIndex;
            }
            sprite.sprite = towerData.atlas.GetSprite($"Tower_{spriteIndex}");
            towerGhost = towerData.towerGhostPrefab;
            
        }
    }
    protected virtual void Update()
    {

        if (isMoving)
        {
            curPos = InputManager.Instance.GetTouchWorldPosition();
            towerGhost.transform.position = curPos;
        }


        //if (Time.time - lastCheckTime < towerData.AttackSpeed) return;
        //{
        //    FindTarget();
        //    if (projectileFactory == null || towerData == null)
        //    {
        //        Debug.LogError("ProjectileFactory or TowerData is null in BaseTower.Update");
        //        return;  // 필수 객체가 null이라면 Update에서 더 이상 진행하지 않음
        //    }
        //    lastCheckTime = Time.time;
        //    Attack();
        //}
    }


    //bool IsInRange(Vector3 targetPos)
    //{
    //    return Vector3.Distance(transform.position, targetPos) <= towerData.AttackRange;
    //}

    //void FindTarget()
    //{
    //    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange, monsterLayer);

    //    float closestDist = float.MaxValue;
    //    Transform closest = null;

    //    foreach (var hit in hits)
    //    {
    //        float dist = Vector2.Distance(transform.position, hit.transform.position);
    //        if (dist < closestDist)
    //        {
    //            closestDist = dist;
    //            closest = hit.transform;
    //        }
    //    }
    //    if (target == closest) return;
    //    if (currentTargetMonster != null)
    //    {
    //        currentTargetMonster.OnMonsterDeathAction -= HandleTargetDeath;
    //    }
    //    target = closest;
    //    currentTargetMonster = target.GetComponent<BaseMonster>();
    //    if (currentTargetMonster != null)
    //    {
    //        currentTargetMonster.OnMonsterDeathAction += HandleTargetDeath; 
    //    }
    //}

    //void Attack()
    //{
    //    if (target == null || !IsInRange(target.position))return;
    //    Debug.Log($"[BaseTower] {towerData.TowerName} 공격대상: {target.name}");
    //    //projectileFactory.SpawnAndLaunch(target.position,towerData,this.transform);
    //    switch (towerData.ProjectileType)
    //    {
    //        case ProjectileType.Magic:
    //            projectileFactory.SpawnAndLaunch<MagicProjectile>(target.position, towerData, this.transform);
    //            break;
    //        case ProjectileType.Blast:
    //            projectileFactory.SpawnAndLaunch<BlastProjectile>(target.position, towerData, this.transform);
    //            break;
    //        case ProjectileType.Arrow:
    //            projectileFactory.SpawnAndLaunch<ArrowProjectile>(target.position, towerData, this.transform);
    //            break;
    //        default:
    //            Debug.LogError($"[BaseTower] {towerData.TowerName} 공격타입 없음");
    //            break;
    //    }
    //}

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

    //private void HandleTargetDeath()
    //{
    //    Debug.Log($"[BaseTower] {towerData.TowerName} 공격대상 사망");
    //    target = null;
    //    lastCheckTime = Time.time;
    //    currentTargetMonster.OnMonsterDeathAction -= HandleTargetDeath;
    //    currentTargetMonster = null;
    //}
    protected virtual void OnDestroy()
    {
        InputManager.Instance?.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
        //if (currentTargetMonster != null)
        //{
        //    currentTargetMonster.OnMonsterDeathAction -= HandleTargetDeath;
        //    currentTargetMonster = null;
        //}
        isMoving = false;
    }
}


