using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public static class AnalyticsLogger
{
    public static bool IsServiceOn = true;

    public static bool ShouldSkipAnalytics()
    {
        //작동 중지
        if (!IsServiceOn || UnityServices.State != ServicesInitializationState.Initialized)
            return true;
        //작동
        return false;
    }

    public static void LogSelectStartCard(List<TowerData> data)
    {
        if (ShouldSkipAnalytics()) return;
       
        for (int i = 0; i < data.Count; i++)
        {
            var customEvent = new CustomEvent(i < 3 ? "Card_Selection_Page1" : "Card_Selection_Page2");
            customEvent.Add("card_index", data[i].TowerIndex);

            AnalyticsService.Instance.RecordEvent(customEvent);
        }
        Debug.Log("카드 전송 완료");
    }

    public static void LogSelectAttackType(string attackType)
    {
        if (ShouldSkipAnalytics()) return;
        var customEvent = new CustomEvent("Select_AttackType");
        customEvent.Add("attackType", attackType);
        AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log($"선택 타입 = {attackType}");
    }

    public static void LogEndGameWave(bool gameOver, int wave)
    {
        if (ShouldSkipAnalytics()) return;
        var customEvent = new CustomEvent("EndGameWave");
        if (gameOver) customEvent.Add("GameOverWave", wave);
        else customEvent.Add("GiveupWave", wave);
            AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log($"게임 종료 = {(gameOver ? "GameOver" : "Giveup")} / 웨이브 = {wave}");
    }

    public static void LogUpgradeLevel(TowerUpgradeData upgradeData)
    {
        if (ShouldSkipAnalytics()) return;

        for(int i = 0; i < upgradeData.currentLevel.Count; i++)
        {
            var customEvent = new CustomEvent("Upgrade_Level_Distribution");

            string type = ((TowerUpgradeType)i).ToString();
            int level = upgradeData.currentLevel[i];

            customEvent.Add("upgrade_type", type);
            customEvent.Add("level", level);

            AnalyticsService.Instance.RecordEvent(customEvent);
        }
    }

}
