using UnityEngine;

public class EffectChangeSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }

    /// <summary>
    /// 스프라이트를 설정하고 위치 및 크기를 지정합니다. (사각형 또는 일반적 사용)
    /// </summary>
    public void ShowSquare(string spritePath, Vector2 center, float sizeX, float sizeY)
    {
        if (spriteRenderer != null) spriteRenderer.enabled = true;

        Sprite sprite = Resources.Load<Sprite>(spritePath);
        if (sprite == null)
        {
            Debug.LogWarning("스프라이트 로드 실패: " + spritePath);
            return;
        }

        spriteRenderer.sprite = sprite;
        transform.position = center;
        transform.localScale = new Vector2(sizeX, sizeY);
    }

    /// <summary>
    /// 원형 범위 스프라이트를 설정합니다. (반지름 하나로 크기 지정)
    /// </summary>
    public void ShowCircle(string spritePath, Vector2 center, float radius)
    {
        spriteRenderer.enabled = true;

        Sprite sprite = Resources.Load<Sprite>(spritePath);
        if (sprite == null)
        {
            Debug.LogWarning("스프라이트 로드 실패: " + spritePath);
            return;
        }

        spriteRenderer.sprite = sprite;
        transform.position = center;
        transform.localScale = new Vector2(radius, radius);
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }
}
