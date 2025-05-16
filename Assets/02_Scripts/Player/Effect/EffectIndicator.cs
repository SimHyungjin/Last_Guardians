using System.Collections;
using UnityEngine;

public class EffectIndicator : MonoBehaviour, IPoolable
{
    public EffectChangeSprite effectChangeSprite;
    public EffectChangeMesh effectChangeMesh;

    Coroutine destroyCoroutine;

    float destroyTime = 0.2f;

    private void Awake()
    {
        effectChangeSprite = GetComponent<EffectChangeSprite>();
        effectChangeMesh = GetComponent<EffectChangeMesh>();
    }

    public void OnSpawn()
    {
        effectChangeSprite.Hide();
        effectChangeMesh.Hide();

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
        effectChangeSprite.Hide();
        effectChangeMesh.Hide();
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(destroyTime);
        PoolManager.Instance.Despawn(this);
    }
}
