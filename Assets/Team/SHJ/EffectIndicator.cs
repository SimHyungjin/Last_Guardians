using System.Collections;
using UnityEngine;

public class EffectIndicator : MonoBehaviour, IPoolable
{
    SpriteRenderer spriteRenderer;

    Coroutine destroyCoroutine;

    float destroyTime = 0.2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnSpawn()
    {
        destroyCoroutine = null;
        destroyCoroutine = StartCoroutine(DestroyThis());
    }

    public void OnDespawn()
    {
        if (destroyCoroutine != null)
        {
            StopCoroutine(destroyCoroutine);
            destroyCoroutine = null;
        }
    }

    public void ChangeSpriteBox(string effectSpriteName, Vector2 cneter, float sizeX, float sizeY)
    {
        Sprite sprite = Resources.Load<Sprite>(effectSpriteName);
        if (sprite == null)
        {
            Debug.LogWarning("스프라이트 로드 실패: " + effectSpriteName);
            return;
        }
        spriteRenderer.sprite = sprite;

        transform.position = cneter;
        transform.localScale = new Vector2 (sizeX, sizeY);
    }

    public void ChangeSpriteCircle(string effectSpriteName, Vector2 cneter, float radius)
    {
        Sprite sprite = Resources.Load<Sprite>(effectSpriteName);
        if (sprite == null)
        {
            Debug.LogWarning("스프라이트 로드 실패: " + effectSpriteName);
            return;
        }
        spriteRenderer.sprite = sprite;

        transform.position = cneter;
        transform.localScale = new Vector2 (radius,radius);
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(destroyTime);
        PoolManager.Instance.Despawn(this);
    }
}
