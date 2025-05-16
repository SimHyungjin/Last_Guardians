// TowerIconContainer.cs
using UnityEngine;

public class TowerIconContainer : MonoBehaviour
{
    public static TowerIconContainer Instance { get; private set; }

    [Tooltip("인덱스별 스프라이트 (Utils.GetSpriteIndex값 -1 순서)")]
    public Sprite[] TowerIcons;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// towerIndex를 Utils.GetSpriteIndex로 보정한 뒤, 배열에서 꺼내 리턴
    /// </summary>
    public Sprite GetSprite(int towerIndex)
    {
        int adjusted = Utils.GetSpriteIndex(towerIndex) - 1;
        if (adjusted >= 0 && adjusted < TowerIcons.Length)
            return TowerIcons[adjusted];
        return null;
    }
}
