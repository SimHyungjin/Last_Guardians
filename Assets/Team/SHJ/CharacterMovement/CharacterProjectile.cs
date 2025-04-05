using UnityEngine;

public class CharacterProjectile : MonoBehaviour
{
    private float speed = 10f;
    private float lifeTime = 5f;
    private float multiSpread = 1f;

    private float damage;
    private Vector2 direction;

    private bool isMulti = false;

    public void Launch(Vector2 targetPos, float _damage, bool _isMulti = false)
    {
        damage = _damage;
        direction = (targetPos - (Vector2)transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        isMulti = _isMulti;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("부딫침");
        if (((1 << collision.gameObject.layer) & (1 << LayerMask.NameToLayer("Monster"))) == 0)
        {
            Debug.Log("레이어 문제");
            return;
        }

        if (isMulti)
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, multiSpread, LayerMask.GetMask("Monster"));
            foreach (var hit in hits)
            {
                // 데미지 처리 및 이펙트는 몬스터 개발자 연동 예정
                Debug.Log("멀티 불릿 공격" + hit.name);
            }
        }
        else
        {
            // 데미지 처리 및 이펙트는 몬스터 개발자 연동 예정
            Debug.Log("단일 불릿 공격");
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
