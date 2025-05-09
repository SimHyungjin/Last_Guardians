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
        disappearCorutine = null;
        InGameManager.Instance.GetExp((int)EXP/2);
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

        timer += Time.deltaTime;
        if (timer >= Interval)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (disappearCorutine != null)
                {
                    StopCoroutine(disappearCorutine);
                    disappearCorutine = null;
                }
                InGameManager.Instance.GetExp(EXP);
                PoolManager.Instance.Despawn<EXPBead>(this);
            }
        }
    }
}
