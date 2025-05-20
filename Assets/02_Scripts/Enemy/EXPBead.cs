using System.Collections;
using UnityEngine;

public class EXPBead : MonoBehaviour
{
    private Collider2D collider;
    private Coroutine disappearCorutine;
    private float disTime = 15f;
    public int EXP { get; private set; }

    private bool isCollected = false;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        isCollected = false;
        //경험치 구슬 사라지는 코루틴
        disappearCorutine = StartCoroutine(Disappear());
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disTime);
        disappearCorutine = null;
        //경험치 반만 획득
        InGameManager.Instance.GetExp((int)EXP / 2);
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
            PoolManager.Instance.Despawn<EXPBead>(this);
        }
    }

}
