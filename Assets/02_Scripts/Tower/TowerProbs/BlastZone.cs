using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlastZone : MonoBehaviour
{
    ///////////==========================BlastProject폭발시 바닥에 표시되는 장판================================/////////////////////

    public void Init(TowerData towerData,Transform _blastpos)
    {
        switch (towerData.ElementType)
        {
            case ElementType.Fire:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case ElementType.Water:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case ElementType.Earth:
                GetComponent<SpriteRenderer>().color = Color.grey;
                break;
            case ElementType.Wind:
                GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            case ElementType.Light:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case ElementType.Dark:
                GetComponent<SpriteRenderer>().color = Color.black;
                break;
            default:
                break;
        }

#if UNITY_EDITOR
    string spritename = $"{towerData.ElementType}BlastEffect";
        string path = $"Assets/91_Disign/Sprite/ProjectileImage/BlastEffects/{spritename}.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        GetComponent<SpriteRenderer>().sprite = sprite;
#endif
        StartCoroutine(BlastEffect());
        Debug.Log($"폭발위치 {_blastpos}");
        transform.position = _blastpos.position;
        Debug.Log($"생성위치 {transform.position}");
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

