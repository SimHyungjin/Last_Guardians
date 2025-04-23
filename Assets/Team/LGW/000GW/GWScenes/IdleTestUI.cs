using System;
using UnityEngine;

public class IdleTestUI : MonoBehaviour
{
    public void AddMinuteTest() // 5분 뒤로 조작
    {
        DateTime now = DateTime.Now.AddMinutes(-5);
        PlayerPrefs.SetString("LastActiveTime", now.ToString());
        Debug.Log("[Test] 방치 시간 5분 세팅");
    }

    public void ForceReward() // 강제로 보상 지급
    {
        IdleRewardManager.Instance.SendMessage("GrantOfflineRewards");
    }
}
