using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public static class AnalyticsLogger
{
    public static bool ShouldSendAnalytics()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        return true;
#else
        return true;
#endif
    }

    public static void LogSelectStartCard(List<TowerData> data)
    {
        if (ShouldSendAnalytics()) return;
       
        for (int i = 0; i < data.Count; i++)
        {
            var customEvent = new CustomEvent("Card_Selection_v2");

            string paramName = i < 3 ? "card_index_page1" : "card_index_page2";
            customEvent.Add(paramName, data[i].TowerIndex);

            AnalyticsService.Instance.RecordEvent(customEvent);
        }
        Debug.Log("카드 전송 완료");
    }

    public static void LogSelectAttackType(string attackType)
    {
        if (ShouldSendAnalytics()) return;
        var customEvent = new CustomEvent("Select_AttackType");
        customEvent.Add("attackType", attackType);
        AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log($"선택 타입 = {attackType}");
    }

    public static void LogGameOverWave(int wave)
    {
        if (ShouldSendAnalytics()) return;
        var customEvent = new CustomEvent("GameOverWave");
        customEvent.Add("EndWave", wave);
        AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log($"선택 타입 = {wave}");
    }



}
