using System.Collections;
using UnityEngine;

public class BlastZone : MonoBehaviour
{
    ///////////==========================BlastProject폭발시 바닥에 표시되는 장판================================/////////////////////

    public void Init(TowerData towerData, Transform _blastpos)
    {
        Utils.GetColor(towerData, GetComponent<SpriteRenderer>());
        StartCoroutine(BlastEffect());
        transform.position = _blastpos.position;
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

