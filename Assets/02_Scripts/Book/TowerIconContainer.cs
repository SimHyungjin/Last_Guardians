using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerIconContainer : MonoBehaviour
{
    public Sprite[] TowerIcons;

    public Sprite GetSprite(int towerindex)
    {
        int adjustedIndex = Utils.GetSpriteIndex(towerindex);
        adjustedIndex = adjustedIndex - 1;
        if (adjustedIndex >= 0 && adjustedIndex < TowerIcons.Length)
            return TowerIcons[adjustedIndex];
        else
            return null;
    }
}
