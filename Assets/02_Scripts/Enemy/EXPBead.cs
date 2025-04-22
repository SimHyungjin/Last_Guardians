using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPBead : MonoBehaviour
{
    private Collider2D collider;
    public int EXP { get; private set; }
    private Coroutine disappearCorutine;
    private float disTime = 30f;


    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        disappearCorutine = StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disTime);
        PoolManager.Instance.Despawn<EXPBead>(this);
    }

    public void Init(int exp, Transform monster)
    {
        EXP = exp;
        this.transform.position = monster.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.timeScale == 0)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            InGameManager.Instance.GetExp(EXP);
            PoolManager.Instance.Despawn<EXPBead>(this);
        }
    }

    private void OnDisable()
    {
        StopCoroutine(disappearCorutine);
        disappearCorutine = null;
    }
}
