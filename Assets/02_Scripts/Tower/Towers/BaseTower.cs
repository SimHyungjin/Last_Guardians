using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TowerUpgradeData towerUpgradeData;
    public EnvironmentEffect environmentEffect;

    [Header("타워 결합")]
    public bool isClicked;
    public SpriteRenderer sprite;

    [Header("타워 외관")]
    protected Animator animator;
    private GradeStar gradeStar;
    [SerializeField]private TextMeshPro towerNameText;

    Vector2 curPos;

    protected float maxbuffRadius = 3f;
    protected bool disable;

    private float touchTime = 0.5f;
    private bool hasTriggeredHold = false;
    private Coroutine holdCheckCoroutine;

    /// <summary>
    /// 타워의 데이터 초기화
    /// </summary>
    /// <param name="_towerData"></param>
    public virtual void Init(TowerData _towerData)
    {
        towerUpgradeData= Resources.Load<TowerUpgradeData>("SO/Tower/towerUpgrade/towerUpgrade");
        towerData = _towerData;
        InputManager.Instance?.BindTouchPressed(OnTouchStart, OnTouchEnd);
        sprite = GetComponent<SpriteRenderer>();
        TowerManager.Instance.AddTower(this);

        OverrideAnimator();
        sprite.sprite = TowerManager.Instance.GetSprite(towerData.TowerIndex);

        gradeStar = GetComponentInChildren<GradeStar>();
        gradeStar.Init(towerData.UpgradeLevel);

        towerNameText = GetComponentInChildren<TextMeshPro>();
        if (EnviromentManager.Instance.Season == Season.winter)
        {
            towerNameText.color = Color.black;
        }
        towerNameText.text = towerData.TowerName;
        towerNameText.gameObject.SetActive(false);

        environmentEffect = new EnvironmentEffect();
        environmentEffect.ClearEffect();

        StartCoroutine(AfterInit());
    }

    /// <summary>
    /// 초기화 이후 한프레임 대기 후 설치위치에 있는 장애물과 트랩을 체크한다.
    /// </summary>
    /// <returns></returns>
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

    ///////////=====================터치관련=====================================/////////////////////
    
    /// <summary>
    /// 터치시 호출된다.
    /// 일반터치와 홀드터치로 나뉘어져있다.
    /// 일반터치시 타워의 이름을 보여준다.
    /// 홀드터치시 타워고스트를 이동시키며(타워빌더에서 처리) 합성여부반별을한다
    /// </summary>
    /// <param name="ctx"></param>
    private void OnTouchStart(InputAction.CallbackContext ctx)
    {
        if (this == null) return;
        if (!isClicked)
        {
            curPos = InputManager.Instance.GetTouchWorldPosition();
            Collider2D hit = Physics2D.OverlapPoint(curPos, LayerMaskData.tower);
            if (hit != null && hit.gameObject == this.gameObject && TowerManager.Instance.CanStartInteraction())
            {
                Debug.Log("Tower Touch Start");
                isClicked = true;
                hasTriggeredHold = false;
                if (holdCheckCoroutine != null) StopCoroutine(holdCheckCoroutine);
                holdCheckCoroutine = StartCoroutine(CheckHoldTrigger());
            }
        }
    }

    /// <summary>
    /// 터치가 끝났을때 호출된다.
    /// 일반터치시 타워의 이름을 보여준다.
    /// 홀드터치시 합성가능하면 합성후 타워를 파괴하고, 합성불가능하면 원상복구한다.
    /// </summary>
    /// <param name="ctx"></param>
    private void OnTouchEnd(InputAction.CallbackContext ctx)
    {
        if (this == null) return;
        if (isClicked)
        {
            if (!hasTriggeredHold)
            {
                isClicked = false;
                ShowTowerName();
            }
            else
            {
                isClicked = false;
                TowerManager.Instance.towerbuilder.EndAttackRangeCircle();
                TowerManager.Instance.towerbuilder.ChangeTowerDontMove(this);
                TowerManager.Instance.EndInteraction(InteractionState.TowerMoving);
                if (TowerManager.Instance.towerbuilder.CanTowerToTowerCombine(InputManager.Instance.GetTouchWorldPosition()))
                {
                    TowerManager.Instance.towerbuilder.TowerToTowerCombine(InputManager.Instance.GetTouchWorldPosition());
                }

            }

        }
    }

    /// <summary>
    /// 터치가 touchTime이상이되면 홀드터치로 간주한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckHoldTrigger()
    {
        yield return new WaitForSeconds(touchTime);

        if (isClicked)
        {
            hasTriggeredHold = true;
            TowerManager.Instance.towerbuilder.ChangeTowerMove(this);
            TowerManager.Instance.StartInteraction(InteractionState.TowerMoving);
            Debug.Log("Hold Activated");
        }
    }

    private void ShowTowerName()
    {
        Debug.Log("Show Tower Name");
        if (towerNameText != null)
        {
            towerNameText.gameObject.SetActive(true);
            DOVirtual.DelayedCall(1f, () => towerNameText.gameObject.SetActive(false));
        }
    }

    protected virtual void OnDisable()
    {
        InputManager.Instance?.UnBindTouchPressed(OnTouchStart, OnTouchEnd);
        TowerManager.Instance.RemoveTower(this);
    }
    protected virtual void OnDestroy()
    {   
        if (TowerManager.Instance.gameObject.activeInHierarchy)
        {
            TowerManager.Instance.StartCoroutine(TowerManager.Instance.NotifyTrapObjectNextFrame(transform.position));
        }
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

    ///////////=====================버프 적용=====================================/////////////////////

    /// <summary>
    /// 타워의 범위내에 있는 타워를 스캔한다.
    /// 범위내에 버프타워가 있다면 재적용한다.
    /// </summary>
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

    /// <summary>
    /// 타워의 범위내에 있는 장애물과 트랩을 스캔한다.
    /// 범위내에 장애물이 있다면 재적용한다.
    /// </summary>
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



