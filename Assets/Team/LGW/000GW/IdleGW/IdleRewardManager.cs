using System;
using UnityEngine;

public class IdleRewardManager : Singleton<IdleRewardManager>
{
    private const int MAX_IDLE_MINUTES = 240;
    private const float REWARD_INTERVAL = 60f;

    private DateTime sessionStartTime;
    private DateTime lastRewardClaimTime;
    private float rewardTimer;

    private int pendingGold;
    private int pendingStone;
    private int pendingEquip;

    private void Start()
    {
        string startTimeStr = PlayerPrefs.GetString("IdleSessionStart", string.Empty);
        string lastClaimStr = PlayerPrefs.GetString("IdleLastClaimTime", string.Empty);

        sessionStartTime = string.IsNullOrEmpty(startTimeStr) ? DateTime.Now : DateTime.Parse(startTimeStr);
        lastRewardClaimTime = string.IsNullOrEmpty(lastClaimStr) ? sessionStartTime : DateTime.Parse(lastClaimStr);

        pendingGold = PlayerPrefs.GetInt("IdlePendingGold", 0);
        pendingStone = PlayerPrefs.GetInt("IdlePendingStone", 0);
        pendingEquip = PlayerPrefs.GetInt("IdlePendingEquip", 0);

        ApplyOfflineRewards();
    }

    private void Update()
    {
        rewardTimer += Time.deltaTime;

        // �ǽð� ���� ���: 1�и���
        while (rewardTimer >= REWARD_INTERVAL)
        {
            rewardTimer -= REWARD_INTERVAL;
            AccumulateReward();
        }
    }

    private void AccumulateReward()
    {
        System.Random rng = new();

        int gold = rng.Next(20, 31);
        pendingGold += gold;
        if (rng.NextDouble() < 0.10) pendingStone++;
        if (rng.NextDouble() < 0.05) pendingEquip++;

        SavePendingRewards();
        Debug.Log($"[�ǽð� ����] ���+{gold}, ��ȭ��:{pendingStone}, ���:{pendingEquip}");
    }

    private void ApplyOfflineRewards()
    {
        TimeSpan offlineTime = DateTime.Now - lastRewardClaimTime;
        int minutes = Mathf.Min((int)offlineTime.TotalMinutes, MAX_IDLE_MINUTES);

        if (minutes <= 0) return;

        System.Random rng = new();
        for (int i = 0; i < minutes; i++)
        {
            pendingGold += rng.Next(20, 31);
            if (rng.NextDouble() < 0.10) pendingStone++;
            if (rng.NextDouble() < 0.05) pendingEquip++;
        }

       
        lastRewardClaimTime = DateTime.Now;

        SaveAll();

        Debug.Log($"[�������� ����] {minutes}�� �� ���+{pendingGold}, ��ȭ��+{pendingStone}, ���+{pendingEquip}");
    }


    public void ClaimReward()
    {
        if (pendingGold > 0) SaveSystem.SaveGoldReward(pendingGold);
        if (pendingStone > 0) SaveSystem.SaveUpgradeStonedReward(pendingStone);
        for (int i = 0; i < pendingEquip; i++)
            SaveSystem.SaveEquipReward(0); // ���� ��� �ε����� ��ü ����

        
        Debug.Log($"[���� ���� �Ϸ�] ���: {pendingGold}, ��ȭ��: {pendingStone}, ���: {pendingEquip}");

        // ���� ��ġ �ʱ�ȭ
        pendingGold = 0;
        pendingStone = 0;
        pendingEquip = 0;

        // Ÿ�̸� �ʱ�ȭ
        lastRewardClaimTime = DateTime.Now;
        sessionStartTime = DateTime.Now;

        SaveAll();
        SaveSystem.SaveGame();
    }


    private void SavePendingRewards()
    {
        PlayerPrefs.SetInt("IdlePendingGold", pendingGold);
        PlayerPrefs.SetInt("IdlePendingStone", pendingStone);
        PlayerPrefs.SetInt("IdlePendingEquip", pendingEquip);
    }

    private void SaveAll()
    {
        SavePendingRewards();
        PlayerPrefs.SetString("IdleSessionStart", sessionStartTime.ToString());
        PlayerPrefs.SetString("IdleLastClaimTime", lastRewardClaimTime.ToString());
        PlayerPrefs.Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveAll();
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }

    public TimeSpan GetElapsedTime()
    {
        TimeSpan raw = DateTime.Now - sessionStartTime;
        TimeSpan max = TimeSpan.FromMinutes(MAX_IDLE_MINUTES);

        return raw > max ? max : raw;
    }


    public int Gold => pendingGold;
    public int Stone => pendingStone;
    public int Equip => pendingEquip;
}
