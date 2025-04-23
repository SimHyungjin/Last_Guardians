using System;
using UnityEngine;

public class IdleTestUI : MonoBehaviour
{
    public void AddMinuteTest() // 5�� �ڷ� ����
    {
        DateTime now = DateTime.Now.AddMinutes(-5);
        PlayerPrefs.SetString("LastActiveTime", now.ToString());
        Debug.Log("[Test] ��ġ �ð� 5�� ����");
    }

    public void ForceReward() // ������ ���� ����
    {
        IdleRewardManager.Instance.SendMessage("GrantOfflineRewards");
    }
}
