using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils
{
    /// <summary>
    /// Resources 경로에서 프리팹을 로드한 후, 인스턴스화하여 지정한 컴포넌트를 반환합니다.
    /// </summary>
    /// <typeparam name="T">반환할 컴포넌트 타입</typeparam>
    /// <param name="path">Resources 내부의 경로</param>
    /// <param name="parent">인스턴스화할 부모 Transform (선택)</param>
    /// <returns>프리팹에서 추출한 컴포넌트, 없으면 default</returns>
    public static T InstantiateComponentFromResource<T>(string path, Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"리소시스 패스 확인 필요 : {path}");
            return default;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;

        if (go.TryGetComponent<T>(out T component))
        {
            return component;
        }
        else
        {
            return default;
        }
    }
    /// <summary>
    /// Resources 경로에서 프리팹을 로드한 후, GameObject 형태로 인스턴스화합니다.
    /// </summary>
    /// <param name="path">Resources 내부의 경로</param>
    /// <param name="parent">인스턴스화할 부모 Transform (선택)</param>
    /// <returns>생성된 GameObject, 실패 시 null</returns>
    public static GameObject InstantiatePrefabFromResource(string path, Transform parent = null)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"리소시스 패스 확인 필요: {path}");
            return null;
        }
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    /// <summary>
    /// 주변 원 radius 범위에 buffer수만큼 layerMask에 해당하는 콜라이더를 검출하고 Collider2D 배열로 반환
    /// </summary>
    private static Collider2D[] buffer = new Collider2D[20];

    public static Collider2D[] OverlapCircleAllSorted(Vector2 center, float radius, int layerMask)
    {
        int count = Physics2D.OverlapCircleNonAlloc(center, radius, buffer, layerMask);

        return buffer
            .Take(count) // 사용된 요소까지만 정렬
            .ToArray();
    }


    public static bool ShouldApplyEffect(BaseMonster target, TowerData towerData, bool bossImmunebuff)
    {
        if (bossImmunebuff) return true;

        if (towerData.BossImmune && (target.MonsterData.MonsterType == MonType.Boss || target.MonsterData.MonsterType==MonType.Bounty))
        {
            Debug.Log($"[ShouldApplyEffect] {target.name} 보스 면역");
            return false;
        }

        return true;
    }


    public static int GetSpriteIndex(int towerIndex)
    {
        int spriteIndex;
        if (towerIndex > 49 && towerIndex < 99)
        {
            spriteIndex = towerIndex - 49;
        }
        else if (towerIndex >= 99 && towerIndex < 109)
        {
            spriteIndex = towerIndex - 98;
        }
        else if (towerIndex >= 109)
        {
            spriteIndex = towerIndex - 59;
        }
        else
        {
            spriteIndex = towerIndex;
        }
        return spriteIndex;
    }

    public static int GetAnimatorindex(int towerIndex) 
    {
        int spriteIndex;
        if (towerIndex > 49 && towerIndex < 99)
        {
            spriteIndex = towerIndex - 49;
        }
        else if (towerIndex >= 99 && towerIndex < 109)
        {
            spriteIndex = towerIndex - 98;
        }
        else
        {
            spriteIndex = towerIndex;
        }
        return spriteIndex;
    }

    public static void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    public static void GetColor(TowerData towerData, SpriteRenderer spriteRenderer)
    {
        switch (towerData.ElementType)
        {
            case ElementType.Fire:
                spriteRenderer.color = Color.red;
                break;
            case ElementType.Water:
                spriteRenderer.color = Color.blue;
                break;
            case ElementType.Earth:
                spriteRenderer.color = Color.grey;
                break;
            case ElementType.Wind:
                spriteRenderer.color = Color.cyan;
                break;
            case ElementType.Light:
                spriteRenderer.color = Color.yellow;
                break;
            case ElementType.Dark:
                spriteRenderer.color = Color.black;
                break;
            default:
                break;
        }
    }

}
