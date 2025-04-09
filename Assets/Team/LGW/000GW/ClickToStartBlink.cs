using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickToStartBlink : MonoBehaviour
{
    public float blinkDuration = 1f;  

    private void Start()
    {
        Image img = GetComponent<Image>();
        if (img != null)
        {
            img.DOFade(0f, blinkDuration)    
                .SetLoops(-1, LoopType.Yoyo) 
                .SetEase(Ease.InOutSine);    
        }
    }
}