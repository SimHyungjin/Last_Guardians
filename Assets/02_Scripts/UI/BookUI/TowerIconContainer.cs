// TowerIconContainer.cs
using UnityEngine;

public class TowerIconContainer : MonoBehaviour
{
    public static TowerIconContainer Instance { get; private set; }

    [Tooltip("�ε����� ��������Ʈ (Utils.GetSpriteIndex�� -1 ����)")]
    public Sprite[] TowerIcons;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// towerIndex�� Utils.GetSpriteIndex�� ������ ��, �迭���� ���� ����
    /// </summary>
    public Sprite GetSprite(int towerIndex)
    {
        int adjusted = Utils.GetSpriteIndex(towerIndex) - 1;
        if (adjusted >= 0 && adjusted < TowerIcons.Length)
            return TowerIcons[adjusted];
        return null;
    }
}
