using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBountyMonster : MonoBehaviour
{
    private Button spawnBtn;
    [SerializeField] private Image spawnImage;
    [SerializeField] private Image spawnCoolTimer;
    private TextMeshProUGUI spawnText;
    private int spawnIndex;
    

    private void Awake()
    {
        spawnBtn = GetComponent<Button>();
        spawnText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        spawnBtn.onClick.AddListener(SpwanBountyMonster);
        MonsterManager.Instance.spawnAction += ImageTimerStart;

    }

    public void Init(MonsterData data)
    {
        spawnImage.sprite = data.Icon;
        spawnText.text = data.MonsterName;
        spawnIndex = data.MonsterIndex;
    }

    private void SpwanBountyMonster()
    {
        if(MonsterManager.Instance != null && MonsterManager.Instance.SpawnTimer <= 0f)
        {
            MonsterManager.Instance.SpawnBounty(spawnIndex);
            MonsterManager.Instance.StartSpawnTimer();
            MonsterManager.Instance.spawnAction?.Invoke();
        }
    }

    private void ImageTimerStart()
    {
        MonsterManager.Instance.SpawnTimer = MonsterManager.Instance.BountySpwanCoolTime;

        // 이전 Tween 중단
        spawnCoolTimer.DOKill();

        // fillAmount 초기화
        spawnCoolTimer.fillAmount = 1f;

        // Tween으로 부드럽게 줄이기
        spawnCoolTimer.DOFillAmount(0f, MonsterManager.Instance.SpawnTimer).SetEase(Ease.Linear).OnComplete(() =>
        MonsterManager.Instance.SpawnTimer = 0f);
    }
}
