using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlastZone : MonoBehaviour
{
    ///////////==========================BlastProject���߽� �ٴڿ� ǥ�õǴ� ����================================/////////////////////

    public void Init(TowerData towerData,Transform _blastpos)
    {
        Utils.GetColor(towerData, GetComponent<SpriteRenderer>());
        StartCoroutine(BlastEffect());
        Debug.Log($"������ġ {_blastpos}");
        transform.position = _blastpos.position;
        Debug.Log($"������ġ {transform.position}");
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

