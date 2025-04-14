using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MulliganCard : MonoBehaviour
{
    private int towerIndex;
    [SerializeField] private SpriteAtlas atlas;

    public void Init(int index)
    {
        towerIndex = index;
        GetComponent<Image>().sprite = atlas.GetSprite($"Card_{index}");
    }
}
