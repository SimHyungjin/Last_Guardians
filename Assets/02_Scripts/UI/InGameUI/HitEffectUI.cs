using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class HitEffectUI : MonoBehaviour
{
    [SerializeField] private Material edgeFlashMaterial;
    private float currentIntensity = 0f;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (edgeFlashMaterial != null)
        {
            edgeFlashMaterial.SetFloat("_Intensity", currentIntensity);
            Graphics.Blit(src, dest, edgeFlashMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    public void PlayHitEffect(float intensity = 0.7f, float fadeTime = 0.3f)
    {
        DOTween.Kill(this); // 기존 트윈이 있다면 종료
        DOTween.To(() => currentIntensity, x => currentIntensity = x, intensity, 0f)
            .SetId(this) // 트윈 ID 설정
            .OnUpdate(() => edgeFlashMaterial.SetFloat("_Intensity", currentIntensity))
            .OnComplete(() => edgeFlashMaterial.SetFloat("_Intensity", 0));

        DOTween.To(() => currentIntensity, x => currentIntensity = x, 0f, fadeTime)
            .SetEase(Ease.OutQuad)
            .SetId(this)
            .OnUpdate(() => edgeFlashMaterial.SetFloat("_Intensity", currentIntensity));
    }
}
