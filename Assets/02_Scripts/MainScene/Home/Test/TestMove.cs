using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMove : MonoBehaviour
{
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        Move();
    }
    public void Move()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(90, 90, 0), 1f))
            .Append(transform.DORotate(new Vector3(0, 0, 0), 1f))
            .Join(transform.DOMove(new Vector3(1170, 540, 0), 1f))
            .SetEase(Ease.InOutSine);

    }
}
