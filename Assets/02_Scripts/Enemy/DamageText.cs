using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Sequence sequence;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Show(float damage)
    {
        text.text = damage.ToString();
        text.color = Color.blue;

        // 초기 위치 저장
        Vector3 startPos = transform.position;

        // 트윈 시퀀스 실행
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(startPos.y + 1f, 1f).SetEase(Ease.OutCubic))
                .Join(text.DOFade(0f, 1f))
                .OnComplete(() => PoolManager.Instance.Despawn<DamageText>(this));
    }

    private void OnDisable()
    {
        // 꺼질 때 트윈 정리
        if (sequence != null && sequence.IsActive())
        {
            sequence.Kill();
        }
    }
}
