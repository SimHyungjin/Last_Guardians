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
    public GameObject towerGhostPrefab;
    public bool isMoving;
    public bool isCliked;
    public SpriteRenderer sprite;


    private GameObject towerGhost;
    Vector2 curPos;

    protected float maxbuffRadius = 3f;
    protected bool disable;



    public virtual void Init(TowerData _towerData)
    {
        towerData = _towerData;
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        towerGhostPrefab = _towerData.towerGhostPrefab;
        sprite = GetComponent<SpriteRenderer>();
        TowerManager.Instance.AddTower(this);

        if (towerData != null)
        {
            sprite.sprite = towerData.atlas.GetSprite($"Tower_{Utils.GetSpriteIndex(towerData.TowerIndex)}");
            towerGhost = towerData.towerGhostPrefab;       
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

        if (isMoving)
        {
            curPos = InputManager.Instance.GetTouchWorldPosition();
            towerGhost.transform.position = curPos;
        }
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
    public virtual void DestroyBuffTower()
    {

    }
}



