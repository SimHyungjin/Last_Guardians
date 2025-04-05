using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Character character;

    bool isAttacking = false;
    GameObject target;
    float targetCheckTime = 0.2f;

    private float responsiveness = 0.5f;
    private Vector2 startPos;
    private bool isSwiping = false;

    private Coroutine dragCoroutine;
    private Coroutine attackCoroutine;


    public void Init(Character _character)
    {
        character = _character;
        AutoAttackStart();
    }

    void Attack(Vector2 targetMonster)
    {
        if (isAttacking) return;
        isAttacking = true;
        target = null;
        Debug.Log("공격");
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(character.player.attackSpeed);
        isAttacking = false;
    }

    float CalculateDamage()
    {
        float damage = character.player.attackPower;
        float randomFloat = Random.Range(0, 100);

        if (randomFloat <= character.player.criticalChance)
        {
            damage *= character.player.criticalDamage;
        }
        return damage;
    }

    public void AutoAttackStart()
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(FindEnemy());
        }
    }      
    public void AutoAttackStop()
    {
        if (attackCoroutine == null) return;
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;
    }
    IEnumerator FindEnemy()
    {
        int layer = LayerMask.NameToLayer("Monster");
        int layerMask = 1 << layer;

        while(true)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, character.player.attackRange, layerMask);

            if (hits.Length == 0)
                Debug.LogWarning("몬스터를 찾지 못하였습니다");

            else
            {
                float minDsitance = float.MaxValue;
                foreach (var hit in hits)
                {
                    float distance = (hit.transform.position - transform.position).magnitude;
                    if (distance < minDsitance)
                    {
                        minDsitance = distance;
                        target = hit.gameObject;
                    }
                }
                if(target != null && target.activeInHierarchy)
                {
                    Attack(target.transform.position);
                }
            }
            yield return new WaitForSeconds(targetCheckTime);
        }
    }

    //test용
    GameObject testObj;
    //test용

    public void SwipeStart()
    {
        if (isSwiping) return;
        startPos = InputManager.Instance.GetTouchWorldPosition();
        isSwiping = true;

        //test용
        testObj = new GameObject("MouseTestObj");
        var sr = testObj.AddComponent<SpriteRenderer>();
        sr.color = Color.white;
        var sprite = Resources.Load<Sprite>("MouseTest");
        if (sprite != null)
            sr.sprite = sprite;
        else
            Debug.LogWarning("MouseTest 스프라이트를 찾을 수 없습니다.");
        testObj.transform.position = startPos;
        //test용

        dragCoroutine = StartCoroutine(DragLoop());
    }

    public void SwipeStop()
    {
        if (!isSwiping) return;
        isSwiping = false;

        if (dragCoroutine != null)
        {
            //test용
            if (testObj != null) Destroy(testObj);
            //test용

            StopCoroutine(dragCoroutine);
            dragCoroutine = null;
        }
    }

    private IEnumerator DragLoop()
    {
        while (isSwiping)
        {
            Vector2 curPos = InputManager.Instance.GetTouchWorldPosition();
            Vector2 dir = curPos - startPos;

            float distance = dir.magnitude;

            if (distance >= responsiveness)
            {
                dir = dir.normalized;
                float speedFactor = Mathf.Clamp(distance / 3, 0f, 1f);

                character.transform.position += (Vector3)dir * character.player.moveSpeed * speedFactor * Time.deltaTime;
            }
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (character == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, character.player.attackRange);
    }
}
