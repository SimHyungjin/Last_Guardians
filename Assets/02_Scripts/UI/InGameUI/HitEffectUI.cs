using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffectUI : MonoBehaviour
{
    [SerializeField] private Image hitImage;
    [SerializeField] private float flashAlpha = 0.5f;      // 피격 시 알파
    [SerializeField] private float fadeDuration = 0.3f;    // 페이드 지속 시간

    private void Awake()
    {
        if (hitImage != null)
            hitImage.color = new Color(1, 0, 0, 0); // 투명 빨강으로 초기화
    }

    public void PlayHitEffect()
    {
        if (InGameManager.Instance.isGameOver)
            return;

        hitImage.color = new Color(1, 0, 0, flashAlpha); // 알파 적용된 빨강
        hitImage.DOFade(0, fadeDuration);                // 점점 투명하게
    }
}
