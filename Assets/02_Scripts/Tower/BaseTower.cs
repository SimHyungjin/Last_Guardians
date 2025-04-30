using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EnvironmentEffect
{
    public bool isNearWater;
    public bool isNearFire;
    public bool isBuffAffectedByWater;
    public bool isBuffAffectedByFire;

    public void ClearEffect()
    {
        isNearWater = false;
        isNearFire = false;
        isBuffAffectedByWater = false;
        isBuffAffectedByFire = false;
    }
    public bool IsFireBoosted()=> isNearFire || isBuffAffectedByFire;

    public bool IsWaterBoosted()=> isNearWater || isBuffAffectedByWater;

}

public abstract class BaseTower : MonoBehaviour
{
    [Header("타워 데이터")]
    public TowerData towerData;
    public EnvironmentEffect environmentEffect;

    [Header("타워 결합")]
    public bool isMoving;
    public bool isCliked;
    public SpriteRenderer sprite;

    [Header("타워 애니메이션")]
    protected Animator animator;


    Vector2 curPos;

    protected float maxbuffRadius = 3f;
    protected bool disable;

    protected float touchTime = 1f;
    protected float touchStartTime = 0f;

    public virtual void Init(TowerData _towerData)
    {
        towerData = _towerData;
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        sprite = GetComponent<SpriteRenderer>();
        TowerManager.Instance.AddTower(this);

        if (towerData != null)
        {
            OverrideAnimator();
            sprite.sprite = TowerManager.Instance.GetSprite(towerData.TowerIndex);
        }

        environmentEffect = new EnvironmentEffect();
        environmentEffect.ClearEffect();

        StartCoroutine(AfterInit());
    }

    protected IEnumerator AfterInit()
    {
        yield return null;
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position, LayerMaskData.floor);
        foreach (var hit in hits)
        {
            if (hit.transform.IsChildOf(this.transform)) continue;
            TrapObject trapObject = hit.GetComponent<TrapObject>();
            BaseObstacle basePlantedObstacle = hit.GetComponent<BaseObstacle>();
            PlantedEffect plantedEffect = hit.GetComponent<PlantedEffect>();
            if (trapObject != null)
            {
                Debug.Log("설치위치에 트랩있음 다부신다");
                trapObject.ChageState(TrapObjectState.CantActive);
            }
            if (basePlantedObstacle != null)
            {
                Debug.Log("설치위치에 장애물있음 다부신다");
                if(EnviromentManager.Instance.Obstacles.Contains(basePlantedObstacle))
                    EnviromentManager.Instance.Obstacles.Remove(basePlantedObstacle);
                Destroy(basePlantedObstacle.gameObject);
            }
            if (plantedEffect != null)
            {
                switch (plantedEffect.obstacleType)
                {
                    case ObstacleType.Water:
                        Debug.Log("설치위치옆에 물있음");
                        environmentEffect.isNearWater = true;
                        break;
                    case ObstacleType.Fire:
                        Debug.Log("설치위치옆에 불있음");
                        environmentEffect.isNearFire = true;
                        break;

                }

            }
        }
    }
    

    
    protected virtual void Update()
    {

    }

    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
        if (this == null) return;
        if (!isCliked)
        {
            curPos = InputManager.Instance.GetTouchWorldPosition();
            Collider2D hit = Physics2D.OverlapPoint(curPos, LayerMaskData.tower);
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
            TowerManager.Instance.towerbuilder.EndAttackRangeCircle();
            isCliked = false;
            TowerManager.Instance.towerbuilder.ChangeTowerMove(this);
            TowerManager.Instance.EndInteraction(InteractionState.TowerMoving);
            if (TowerManager.Instance.towerbuilder.CanTowerToTowerCombine(InputManager.Instance.GetTouchWorldPosition()))
            {
                TowerManager.Instance.towerbuilder.TowerToTowerCombine(InputManager.Instance.GetTouchWorldPosition());
            }
        }
    }
    protected virtual void OnDisable()
    {
        InputManager.Instance?.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
        TowerManager.Instance.RemoveTower(this);
    }
    protected virtual void OnDestroy()
    {
        isMoving = false;
        if (isActiveAndEnabled)TowerManager.Instance.StartCoroutine(TowerManager.Instance.NotifyTrapObjectNextFrame(transform.position));
    }

    public void OnDisabled()
    {
        disable = true;
        Invoke("OnEnabled", 3.5f);
    }
    public void OnEnabled()
    {
        disable = false;
    }

    protected void ScanBuffTower()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxbuffRadius, LayerMaskData.tower);

        foreach (var hit in hits)
        {
            BuffTower otherTower = hit.GetComponent<BuffTower>();
            if (otherTower != null && otherTower != this)
            {
                otherTower.ReApplyBuff();
            }
        }
    }
    public virtual void ScanPlantedObstacle()
    {
        environmentEffect.isNearWater = false;
        environmentEffect.isNearFire = false;

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position, LayerMaskData.obstacleZone);
        foreach (var hit in hits)
        {
            PlantedEffect plantedEffect = hit.GetComponent<PlantedEffect>();
            switch (plantedEffect.obstacleType)
            {
                case ObstacleType.Water:
                    Debug.Log("설치위치옆에 물있음");
                    environmentEffect.isNearWater = true;
                    break;
                case ObstacleType.Fire:
                    Debug.Log("설치위치옆에 불있음");
                    environmentEffect.isNearFire = true;
                    break;

            }
        }
    }
    protected void OverrideAnimator()
    {
        if (animator == null) animator = GetComponent<Animator>();
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController["EmptySpawn"] = towerData.spawnClip;
        overrideController["EmptyIdle"] = towerData.idleClip;
        overrideController["EmptyActive"] = towerData.activeClip;
        animator.runtimeAnimatorController = overrideController;
    }
    public virtual void DestroyBuffTower()
    {

    }
}



