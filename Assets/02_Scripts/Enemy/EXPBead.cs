using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPBead : MonoBehaviour
{
    private Collider2D collider;
    public int EXP { get; private set; }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void Init(int exp, Transform monster)
    {
        EXP = exp;
        this.transform.position = monster.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("경험치 구슬 충돌");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            InGameManager.Instance.GetExp(EXP);
            PoolManager.Instance.Despawn<EXPBead>(this);
        }
    }
}
