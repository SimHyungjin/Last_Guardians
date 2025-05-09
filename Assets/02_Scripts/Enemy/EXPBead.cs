using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPBead : MonoBehaviour
{
    private Collider2D collider;
    private Coroutine disappearCorutine;
    private float disTime = 15f;
    private readonly float Interval = 0.2f;
    private float timer = 0f;
    public int EXP { get; private set; }

    private bool isCollected = false;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        isCollected = false;
        disappearCorutine = StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disTime);
        disappearCorutine = null;
        InGameManager.Instance.GetExp((int)EXP / 2);
        //Debug.Log($"EXPCount : {MonsterManager.Instance.EXPCount}, MonsterKill : {MonsterManager.Instance.MonsterKillCount}");
        PoolManager.Instance.Despawn<EXPBead>(this);
    }

    public void Init(int exp, Transform monster)
    {
        EXP = exp;
        this.transform.position = monster.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.timeScale == 0 || isCollected)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isCollected = true; // 중복 방지

            if (disappearCorutine != null)
            {
                StopCoroutine(disappearCorutine);
                disappearCorutine = null;
            }
            InGameManager.Instance.GetExp(EXP);
            MonsterManager.Instance.EXPCount++;
            Debug.Log($"EXPCount : {MonsterManager.Instance.EXPCount}, MonsterKill : {MonsterManager.Instance.MonsterKillCount}");
            Debug.Log($"경험치 획득 : {EXP}, 총경험치 : {InGameManager.Instance.exp}");
            PoolManager.Instance.Despawn<EXPBead>(this);
        }
    }

}
