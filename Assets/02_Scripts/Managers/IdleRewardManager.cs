using System;
using UnityEngine;

public class IdleRewardManager : Singleton<IdleRewardManager>
{
    private const int MAX_IDLE_MINUTES = 1440; // 24�ð�
    private const float REWARD_INTERVAL_MINUTES = 60f;

    private DateTime sessionStartTime;
    private DateTime lastRewardClaimTime;

    private int pendingGold;
    private int pendingStone;
    private float accumulatedStone = 0f;
    public int Gold => pendingGold;
    public int Stone => pendingStone;

    public TimeSpan TotalElapsed => GetCappedElapsed();
    public TimeSpan NextRewardIn => TimeSpan.FromMinutes(REWARD_INTERVAL_MINUTES) - TimeSpan.FromMinutes(TotalElapsed.TotalMinutes % REWARD_INTERVAL_MINUTES);

    private void Start()
    {
        sessionStartTime = DateTime.Parse(PlayerPrefs.GetString("IdleSessionStart", DateTime.Now.ToString()));
        lastRewardClaimTime = DateTime.Parse(PlayerPrefs.GetString("IdleLastClaimTime", sessionStartTime.ToString()));

        pendingGold = PlayerPrefs.GetInt("IdlePendingGold", 0);
        pendingStone = PlayerPrefs.GetInt("IdlePendingStone", 0);
        

        ApplyOfflineRewards();
    }

    private void Update()
    {
        var elapsed = TotalElapsed.TotalMinutes;
        int rewardCount = Mathf.FloorToInt((float)elapsed / REWARD_INTERVAL_MINUTES);
        int alreadyRewarded = PlayerPrefs.GetInt("IdleRewardCount", 0);

        while (alreadyRewarded < rewardCount)
        {
            GrantReward();
            alreadyRewarded++;
            PlayerPrefs.SetInt("IdleRewardCount", alreadyRewarded);
        }
    }

    private void GrantReward()
    {
        int maxWave = Mathf.Max(1, PlayerPrefs.GetInt("IdleMaxWave", 1));
        float waveBonus = 0.1f * (maxWave / 5f); // ��: 30 �� 0.6

        // ���: 100 �� (1 + ���̺� ���ʽ�)
        int gold = Mathf.RoundToInt(100f * (1f + waveBonus));
        pendingGold += gold;

        // ��ȭ��: ���� ��� (�ð��� 0.5�� �� ���ʽ� ����)
        float stoneGain = 0.5f * (1f + waveBonus);
        accumulatedStone += stoneGain;
        while (accumulatedStone >= 1f)
        {
            pendingStone += 1;
            accumulatedStone -= 1f;
        }

       

        SavePendingRewards();

        Debug.Log($"[���� ����] ���+{gold}, ���� ���:{pendingGold}, ��ȭ��:{pendingStone}");
    }

    private void ApplyOfflineRewards()
    {
        Update();
    }

    public void ClaimReward()
    {
        GameManager.Instance.gold += pendingGold;
        GameManager.Instance.upgradeStones += pendingStone;

       

        Debug.Log($"[���� ���� �Ϸ�] ���: {pendingGold}, ��ȭ��: {pendingStone}");

        pendingGold = 0;
        pendingStone = 0;
        
        accumulatedStone = 0f;

        lastRewardClaimTime = DateTime.Now;
        sessionStartTime = DateTime.Now;
        PlayerPrefs.SetInt("IdleRewardCount", 0);

        
        SaveAll();
        SaveSystem.SaveGame();
    }

    private void SavePendingRewards()
    {
        PlayerPrefs.SetInt("IdlePendingGold", pendingGold);
        PlayerPrefs.SetInt("IdlePendingStone", pendingStone);
        
    }

    private void SaveAll()
    {
        SavePendingRewards();
        PlayerPrefs.SetString("IdleSessionStart", sessionStartTime.ToString());
        PlayerPrefs.SetString("IdleLastClaimTime", lastRewardClaimTime.ToString());
        PlayerPrefs.Save();
    }

    private TimeSpan GetCappedElapsed()
    {
        TimeSpan elapsed = DateTime.Now - sessionStartTime;
        return elapsed.TotalMinutes > MAX_IDLE_MINUTES
            ? TimeSpan.FromMinutes(MAX_IDLE_MINUTES)
            : elapsed;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveAll();
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }
}
