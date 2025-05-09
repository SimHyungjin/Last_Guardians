using System;
using UnityEngine;

public class IdleRewardManager : Singleton<IdleRewardManager>
{
    private const int MAX_IDLE_MINUTES = 1440;
    private const float REWARD_INTERVAL_MINUTES = 60f;

    private DateTime sessionStartTime;
    private int pendingGold;
    private int pendingStone;
    private float accumulatedStone = 0f;

    public int Gold => pendingGold;
    public int Stone => pendingStone;

    public TimeSpan TotalElapsed => GetCappedElapsed();

    // 수정된 NextRewardIn: 60분 배수일 땐 TimeSpan.Zero
    public TimeSpan NextRewardIn
    {
        get
        {
            double rem = TotalElapsed.TotalMinutes % REWARD_INTERVAL_MINUTES;
            if (Math.Abs(rem) < 0.01)
                return TimeSpan.Zero;
            return TimeSpan.FromMinutes(REWARD_INTERVAL_MINUTES - rem);
        }
    }

    private void Start()
    {
        sessionStartTime = DateTime.Parse(
            PlayerPrefs.GetString("IdleSessionStart", DateTime.Now.ToString()));
        pendingGold = PlayerPrefs.GetInt("IdlePendingGold", 0);
        pendingStone = PlayerPrefs.GetInt("IdlePendingStone", 0);
        ApplyOfflineRewards();
    }

    private void Update()
    {
        double elapsed = TotalElapsed.TotalMinutes;
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
        float waveBonus = 0.1f * (maxWave / 5f);

        int goldGain = Mathf.RoundToInt(100f * (1f + waveBonus));
        pendingGold += goldGain;

        float stoneGain = 0.5f * (1f + waveBonus);
        accumulatedStone += stoneGain;
        while (accumulatedStone >= 1f)
        {
            pendingStone++;
            accumulatedStone--;
        }

        SavePendingRewards();
    }

    private void ApplyOfflineRewards() => Update();

    public void ClaimReward()
    {
        // 보상이 하나도 없거나 아직 60분 안 찼으면 무시
        if ((pendingGold == 0 && pendingStone == 0) || NextRewardIn.TotalSeconds > 0)
            return;

        GameManager.Instance.gold += pendingGold;
        GameManager.Instance.upgradeStones += pendingStone;

        pendingGold = 0;
        pendingStone = 0;
        accumulatedStone = 0f;
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
        PlayerPrefs.Save();
    }

    private TimeSpan GetCappedElapsed()
    {
        TimeSpan elapsed = DateTime.Now - sessionStartTime;
        return elapsed.TotalMinutes > MAX_IDLE_MINUTES
            ? TimeSpan.FromMinutes(MAX_IDLE_MINUTES)
            : elapsed;
    }

    private void OnApplicationPause(bool pause) { if (pause) SaveAll(); }
    private void OnApplicationQuit() { SaveAll(); }
}
