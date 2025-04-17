using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlastZone : MonoBehaviour
{
    public void Init(TowerData towerData,Transform _blastpos)
    {
        transform.position = _blastpos.position;
#if UNITY_EDITOR
        string spritename = $"{towerData.ElementType}BlastEffect";
        string path = $"Assets/91_Disign/Sprite/ProjectileImage/BlastEffects/{spritename}.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        GetComponent<SpriteRenderer>().sprite = sprite;
#endif
        StartCoroutine(BlastEffect());
    }
    private IEnumerator BlastEffect()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        PoolManager.Instance.Despawn<BlastZone>(this);
    }
}
