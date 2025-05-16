using System;
using System.Collections;
using UnityEngine;

public class IdleRewardManager : Singleton<IdleRewardManager>
{
    private const int MAX_IDLE_MINUTES = 1440;
    private const float REWARD_INTERVAL_MINUTES = 60f;

    private DateTime sessionStartTime;
    private int pendingGold;
    private int pendingStone;
    private int rewardCount;
    private float accumulatedStone = 0f;

    public int Gold => pendingGold;
    public int Stone => pendingStone;

    public TimeSpan TotalElapsed => GetCappedElapsed();

    public TimeSpan NextRewardIn
    {
        get
        {
            double rem = TotalElapsed.TotalMinutes % REWARD_INTERVAL_MINUTES;
            if (Math.Abs(rem) < 0.01) return TimeSpan.Zero;
            return TimeSpan.FromMinutes(REWARD_INTERVAL_MINUTES - rem);
        }
    }

    protected void Awake()
    {
      
        DontDestroyOnLoad(gameObject);

        // ���� ���� ���� �ε�
        sessionStartTime = DateTime.Parse(
            PlayerPrefs.GetString("IdleSessionStart", DateTime.Now.ToString()));
        pendingGold = PlayerPrefs.GetInt("IdlePendingGold", 0);
        pendingStone = PlayerPrefs.GetInt("IdlePendingStone", 0);
        rewardCount = PlayerPrefs.GetInt("IdleRewardCount", 0);

        // ��� ��׶��� Ÿ�̸� ����
        StartCoroutine(IdleTimerLoop());
    }

    private IEnumerator IdleTimerLoop()
    {
        while (true)
        {
            ProcessRewards();
            yield return new WaitForSeconds(1f);
        }
    }

    private void ProcessRewards()
    {
        // �󸶳� ���� ������ �������� ���
        int totalIntervals = Mathf.FloorToInt(
            (float)TotalElapsed.TotalMinutes / REWARD_INTERVAL_MINUTES
        );

        // ���� ���޵��� ���� ���ݸ�ŭ ���� �ο�
        while (rewardCount < totalIntervals)
        {
            GrantReward();
            rewardCount++;
            PlayerPrefs.SetInt("IdleRewardCount", rewardCount);
        }
    }

    private void GrantReward()
    {
        int maxWave = Mathf.Max(1, PlayerPrefs.GetInt("IdleMaxWave", 1));
        float waveBonus = 0.1f * (maxWave / 5f);

        int goldGain = Mathf.RoundToInt(100f * (1f + waveBonus));
        pendingGold += goldGain;

        accumulatedStone += 0.5f * (1f + waveBonus);
        while (accumulatedStone >= 1f)
        {
            pendingStone++;
            accumulatedStone--;
        }

        SavePendingRewards();
    }

    private void SavePendingRewards()
    {
        PlayerPrefs.SetInt("IdlePendingGold", pendingGold);
        PlayerPrefs.SetInt("IdlePendingStone", pendingStone);
        PlayerPrefs.Save();
    }

    public void ClaimReward()
    {
        if (pendingGold == 0 && pendingStone == 0) return;

        // ���� ����
        GameManager.Instance.gold += pendingGold;
        GameManager.Instance.upgradeStones += pendingStone;
        SoundManager.Instance.PlaySFX("Reward");

        // �ʱ�ȭ
        pendingGold = 0;
        pendingStone = 0;
        accumulatedStone = 0f;
        sessionStartTime = DateTime.Now;
        rewardCount = 0;
        PlayerPrefs.SetInt("IdleRewardCount", 0);

        // ����
        SaveAll();
        SaveSystem.SaveGame();
    }

    private void SaveAll()
    {
        PlayerPrefs.SetString("IdleSessionStart", sessionStartTime.ToString());
        SavePendingRewards();
    }

    private TimeSpan GetCappedElapsed()
    {
        TimeSpan elapsed = DateTime.Now - sessionStartTime;
        return elapsed.TotalMinutes > MAX_IDLE_MINUTES
            ? TimeSpan.FromMinutes(MAX_IDLE_MINUTES)
            : elapsed;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoStartTimer()
    {
        
        var _ = Instance;
    }

    private void OnApplicationPause(bool pause) => SaveAll();
    private void OnApplicationQuit() => SaveAll();
}
