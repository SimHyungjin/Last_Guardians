using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public static class AnalyticsLogger
{
    public static bool IsServiceOn = false;

    public static bool ShouldSendAnalytics()
    {
        //작동 중지
        if (!IsServiceOn || UnityServices.State != ServicesInitializationState.Initialized)
            return true;
        //작동
        return false;
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

<<<<<<< Updated upstream
    public static void LogGameOverWave(int wave)
    {
        if (ShouldSendAnalytics()) return;
        var customEvent = new CustomEvent("GameOverWave");
        customEvent.Add("EndWave", wave);
        AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log($"선택 타입 = {wave}");
    }



=======
    public static void LogEndGameWave(bool gameOver, int wave)
    {
        if (ShouldSendAnalytics()) return;
        var customEvent = new CustomEvent("EndGameWave");
        if (gameOver) customEvent.Add("GameOverWave", wave);
        else customEvent.Add("GiveupWave", wave);
            AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log($"선택 타입 = {wave}");
    }

    public static void LogUpgradeLevel(TowerUpgradeData upgradeData)
    {
        if (ShouldSendAnalytics()) return;

        for(int i = 0; i < upgradeData.currentLevel.Count; i++)
        {
            var customEvent = new CustomEvent("Upgrade_Level_Distribution");
            customEvent.Add($"TowerUpgradeType_{i}",upgradeData.currentLevel[i]);
            AnalyticsService.Instance.RecordEvent(customEvent);
        }
    }
>>>>>>> Stashed changes
}
