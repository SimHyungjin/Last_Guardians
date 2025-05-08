using System;
using UnityEngine;

public class IdleRewardManager : Singleton<IdleRewardManager>
{
    private const int MAX_IDLE_MINUTES = 1440; // 24시간
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
        float waveBonus = 0.1f * (maxWave / 5f); // 예: 30 → 0.6

        // 골드: 100 × (1 + 웨이브 보너스)
        int gold = Mathf.RoundToInt(100f * (1f + waveBonus));
        pendingGold += gold;

        // 강화석: 누적 방식 (시간당 0.5개 × 보너스 포함)
        float stoneGain = 0.5f * (1f + waveBonus);
        accumulatedStone += stoneGain;
        while (accumulatedStone >= 1f)
        {
            pendingStone += 1;
            accumulatedStone -= 1f;
        }

       

        SavePendingRewards();

        Debug.Log($"[보상 지급] 골드+{gold}, 누적 골드:{pendingGold}, 강화석:{pendingStone}");
    }

    private void ApplyOfflineRewards()
    {
        Update();
    }

    public void ClaimReward()
    {
        GameManager.Instance.gold += pendingGold;
        GameManager.Instance.upgradeStones += pendingStone;

       

        Debug.Log($"[보상 수령 완료] 골드: {pendingGold}, 강화석: {pendingStone}");

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
